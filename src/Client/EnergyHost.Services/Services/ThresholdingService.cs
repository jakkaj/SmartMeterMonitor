using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;

namespace EnergyHost.Services.Services
{
    public class ThresholdingService : IThresholdingService
    {
        private readonly ILogService _logService;
        private readonly IDaikinService _daikinService;

        public ThresholdingService(ILogService logService, IDaikinService daikinService)
        {
            _logService = logService;
            _daikinService = daikinService;
        }
        public async Task RunChecks(Dictionary<string, object> data)
        {
            if ((double) data["CurrentPriceIn"] > 45)
            {
                await _powerOffDaikin();
            }
        }

        public async Task _powerOffDaikin()
        {
            await _daikinService.PowerOff();
        }
    }
}
