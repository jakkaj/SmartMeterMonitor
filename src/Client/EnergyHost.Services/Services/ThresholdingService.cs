using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;

namespace EnergyHost.Services.Services
{
    public class ThresholdingService : IThresholdingService
    {
        private readonly ILogService _logService;
        private readonly IDaikinService _daikinService;
        private readonly IOptions<EnergyHostSettings> _settings;

        public ThresholdingService(ILogService logService, IDaikinService daikinService, IOptions<EnergyHostSettings> settings)
        {
            _logService = logService;
            _daikinService = daikinService;
            _settings = settings;
        }
        public async Task RunChecks(Dictionary<string, object> data)
        {
            var threshold = _settings.Value.DaikinThreshold;
            if ((double) data["CurrentPriceIn"] > threshold)
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
