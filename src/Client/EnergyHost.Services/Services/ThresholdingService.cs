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
        private readonly INotificationService _notificationService;

        public ThresholdingService(ILogService logService, IDaikinService daikinService, IOptions<EnergyHostSettings> settings, INotificationService notificationService)
        {
            _logService = logService;
            _daikinService = daikinService;
            _settings = settings;
            _notificationService = notificationService;
        }
        public async Task RunChecks(Dictionary<string, object> data)
        {
            var threshold = _settings.Value.DaikinThreshold;
            
            if (threshold == 0)
            {
                threshold = 60;
            }

            if ((double) data["CurrentPriceIn"] > threshold)
            {
                if (await _powerOffDaikin())
                {
                    await _notificationService.SendNotification(
                        $"Daikin has been powered off as price has spiked. Price is now {data["CurrentPriceIn"]} cents");
                }
            }
        }

        public async Task<bool> _powerOffDaikin()
        {
            return await _daikinService.PowerOff();
        }
    }
}
