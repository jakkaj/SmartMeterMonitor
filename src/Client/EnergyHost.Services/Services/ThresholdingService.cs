using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;

namespace EnergyHost.Services.Services
{
    public class ThresholdingService
    {
        private readonly ILogService _logService;

        public ThresholdingService(ILogService logService)
        {
            _logService = logService;
        }
        public async Task RunChecks(Dictionary<string, object> data)
        {
            if ((int) data["CurrentPriceIn"] > 45)
            {
                await _powerOffDaikin();
            }
        }

        public async Task _powerOffDaikin()
        {
            
        }
    }
}
