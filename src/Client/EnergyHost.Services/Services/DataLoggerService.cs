using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;

namespace EnergyHost.Services.Services
{
    public class DataLoggerService : IDataLoggerService
    {
        private readonly ILogService _logService;
        private readonly IABBService _abbService;
        private readonly IDaikinService _daikinService;

        public DataLoggerService(ILogService logService,
            IDaikinService daikinService,
            IABBService abbService
            )
        {
            _logService = logService;
            _abbService = abbService;
            _daikinService = daikinService;
        }


        public async Task Start()
        {
            _deviceUpdates();
        }
        /// <summary>
        /// Periodically refresh device status 
        /// </summary>
        async void _deviceUpdates()
        {
            while (true)
            {
                var tDaikinSensors = _daikinService.GetSensors();
                
                var tAbbStatus = _abbService.Get();

                var tDaikinStatus = _daikinService.GetStatus();

                await Task.WhenAll(tDaikinSensors, tAbbStatus, tDaikinStatus);

                var abb = await tAbbStatus;
                var daikinStatus = await tDaikinStatus;
                var daikinSensors = await tDaikinSensors;

                var pout = abb?.feeds.Feed.datastreams.m101_1_W.data[0].value * 1000;
               

                var insideTemp = daikinSensors?["htemp"];
                var insideTempSet = daikinStatus?["stemp"];
                var on = daikinStatus?["pow"] == "1";
                var mode = daikinStatus?["mode"];

                

                Debug.WriteLine($"Power: {string.Format("{0:0.00}", pout)} watts, inside temp: {insideTemp}, inside set: {insideTempSet}, power: {on}, mode: {mode}");

                await Task.Delay(TimeSpan.FromSeconds(60));
            }
        }
    }
}
