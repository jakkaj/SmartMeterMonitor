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
        public async Task SendStatus(StatusBase status)
        {

            var ser = JsonConvert.SerializeObject(status);

        }

        public void ReceiveStatus()
        {

        }
    }
}
