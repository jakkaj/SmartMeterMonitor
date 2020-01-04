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

        private double _previousPriceIn = 0;

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

            var testMode = data.ContainsKey("TestMode") && (bool)data["TestMode"];

            if (threshold == 0)
            {
                threshold = 59;
            }

            var currentPriceIn = (double)data["CurrentPriceIn"];

            if ((currentPriceIn > threshold || _previousPriceIn > threshold) && currentPriceIn != _previousPriceIn)
            {
                if (currentPriceIn > threshold)
                {
                    await _notificationService.SendNotification($"Price spike in progress. Current: {currentPriceIn} cents, previous: {_previousPriceIn} cents", "Jorvis - Price Spike Active");
                }

                if (currentPriceIn < threshold && _previousPriceIn > threshold)
                {
                    await _notificationService.SendNotification($"Price spike has ended! Current: {currentPriceIn} cents, previous: {_previousPriceIn} cents", "Jorvis - Price Spike Over!");
                }

                _previousPriceIn = currentPriceIn;

            }

            if ((double)data["CurrentPriceIn"] > threshold)
            {
                var daikinOff = false;
                if (testMode)
                {
                    daikinOff = true;
                }
                else
                {
                    daikinOff = await _powerOffDaikin();
                }

                if (daikinOff)
                {
                    await _notificationService.SendNotification(
                        $"Daikin has been powered off as price has spiked. Price is now {currentPriceIn} cents", "Jorvis");
                }
            }
        }

        public async Task<bool> _powerOffDaikin()
        {
            return await _daikinService.PowerOff();
        }
    }
}
