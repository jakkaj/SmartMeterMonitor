using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnergyHost.Services.Services
{
    public class EventPumpService
    {
        public async Task Boot()
        {

        }

        private async Task timePump()
        {
            while (true)
            {

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}
