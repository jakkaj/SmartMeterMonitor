using System;
using System.Collections.Generic;
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

        public SystemStatusService(ILogService logService, IMQTTService mqttService)
        {
            _logService = logService;
            _mqttService = mqttService;
            _mqttService.EventReceived += _mqttService_EventReceived;
        }

        private void _mqttService_EventReceived(object sender, StatusEventArgs e)
        {
            _logService.WriteLog($"Event: {e.Data}");
        }

        private void _storeStatus()
        {

        }

        public async Task SendStatus(StatusBase status)
        {
            var t = status.GetType();

            var ser = JsonConvert.SerializeObject(status);

            var evt = new EventWrapper
            {
                Data = ser,
                EventName = t.Name
            };
            
            var evtSer = JsonConvert.SerializeObject(evt);

            await _mqttService.Send("events", evtSer);


        }

        public void ReceiveStatus()
        {

        }
    }
}
