using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels.Status;

using EnergyHost.Model.EnergyModels.Status.Base;
using Newtonsoft.Json;

namespace EnergyHost.Services.Services
{
    public class SystemStatusService : ISystemStatusService
    {
        private readonly ILogService _logService;
        private readonly IMQTTService _mqttService;

        private Dictionary<string, object> _statuses { get; } = new Dictionary<string, object>();

        private List<KeyValuePair<string, object>> _history { get; } = new List<KeyValuePair<string, object>>();

        public event EventHandler<StatusUpdatedEventArgs> StatusUpdatedEvent;

        public SystemStatusService(ILogService logService, IMQTTService mqttService)
        {
            _logService = logService;
            _mqttService = mqttService;
            _mqttService.EventReceived += _mqttService_EventReceived;
        }

        private void _mqttService_EventReceived(object sender, StatusEventArgs e)
        {
            _storeStatus(e.Data);
        }

        private void _storeStatus(string statusData)
        {
            try
            {
                var evt = JsonConvert.DeserializeObject<EventWrapper>(statusData);

                var type = $"EnergyHost.Model.EnergyModels.Status.{evt.EventName}, EnergyHost.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                var tActual = Type.GetType(type);

                var decodedType = JsonConvert.DeserializeObject(evt.Data, tActual);

                _statuses[evt.EventName] = decodedType;
                _history.Add(new KeyValuePair<string, object>(evt.EventName, decodedType));

                while (_history.Count > 5000)
                {
                    _history.RemoveAt(_history.Count - 1);
                }

                StatusUpdatedEvent?.Invoke(this, new StatusUpdatedEventArgs{ StatusName = evt.EventName});
            }
            catch (Exception ex)
            {
                _logService.WriteError(ex);
            }
            
        }

        public (T, List<T>) GetStatus<T>()
        where T:StatusBase
        {
            var type = typeof(T);

            if (!_statuses.ContainsKey(type.Name))
            {
                return (null, null);
            }

            var l = _history.Where(_ => _.Key == type.Name).Reverse().Take(10).Select(_=>_.Value as T).ToList();

            l.RemoveAt(0);


            return (_statuses[type.Name] as T, l);
        }

        public async Task SendStatus(StatusBase status, bool useQueue = true)
        {
            var t = status.GetType();

            status.Name = t.Name;
            status.DateTime = DateTime.UtcNow;

            var ser = JsonConvert.SerializeObject(status);

            var evt = new EventWrapper
            {
                Data = ser,
                EventName = t.Name
            };
            
            var evtSer = JsonConvert.SerializeObject(evt);

            if (!useQueue)
            {
               _storeStatus(evtSer);
            }
            else
            {
                await _mqttService.Send("events", evtSer);
            }
        }

        public void ReceiveStatus()
        {

        }
    }
}
