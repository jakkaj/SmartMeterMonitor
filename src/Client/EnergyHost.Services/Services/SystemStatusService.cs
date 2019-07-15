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
        private readonly IMQTTService _mqttService;

        public SystemStatusService(IMQTTService mqttService)
        {
            _mqttService = mqttService;
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
