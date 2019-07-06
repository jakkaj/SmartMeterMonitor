using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels;

namespace EnergyHost.Services.Services
{
    public class DataLoggerService : IDataLoggerService
    {
        private readonly ILogService _logService;
        private readonly IABBService _abbService;
        private readonly IDarkSkyService _darkSkyService;
        private readonly IInfluxService _influxService;
        private readonly IAmberService _amberService;
        private readonly IEnergyFuturesService _energyFuturesService;
        private readonly IMQTTService _mqttService;
        private readonly IDaikinService _daikinService;

        public double SolarOutput { get; set; } = 0;
        public double SolarToday{get;set;} = 0;
        public double DaikinInsideTemperature { get; set; }
        public double DaikinSetTemperature { get; set; }
        public string DaikinMode { get; set; }
        public bool DaikinPoweredOn { get; set; }
        public (double temp, double humid, double pressure,
            double wind, double minToday, double maxToday,
            double minTomorrow, double maxTomorrow) CurrentWeather
        { get; set; }

        public EnergryFutures EnergyFutures { get; set; }
        public double CurrentPriceIn { get; set; }
        public double CurrentPriceOut { get; set; }

        public DataLoggerService(ILogService logService,
            IDaikinService daikinService,
            IABBService abbService,
            IDarkSkyService darkSkyService,
            IInfluxService influxService,
            IAmberService amberService,
            IEnergyFuturesService energyFuturesService,
            IMQTTService mqttService
            )
        {
            _logService = logService;
            _abbService = abbService;
            _darkSkyService = darkSkyService;
            _influxService = influxService;
            _amberService = amberService;
            _energyFuturesService = energyFuturesService;
            _mqttService = mqttService;
            _daikinService = daikinService;
        }


        public async Task Start()
        {
            _deviceUpdates1Mins();

            _deviceUpdates5Mins();

            _abbPoller();

            await _mqttService.Setup();

            await Task.Delay(TimeSpan.FromSeconds(10));

            _dbWriter();
        }

        async void _dbWriter()
        {
            while (true)
            {
                var data = new Dictionary<string, object>
                {
                    { "kwh", _mqttService.KWH },
                    { "temp", CurrentWeather.temp},
                    { "humidity", CurrentWeather.humid},
                    { "pressure", CurrentWeather.pressure},
                    { "windSpeed", CurrentWeather.wind},
                    { "min", CurrentWeather.minToday},
                    { "max", CurrentWeather.maxToday},
                    { "minTomorrow", CurrentWeather.minTomorrow},
                    { "maxTomorrow", CurrentWeather.maxTomorrow},
                    { "DaikinSetTemperature", DaikinSetTemperature },
                    { "DaikinInsideTemperature", DaikinInsideTemperature },
                    { "DaikinMode", DaikinMode },
                    { "DaikinPoweredOn", DaikinPoweredOn },
                    { "SolarOutput", SolarOutput },
                    { "SolarToday", SolarToday},
                    { "CurrentPriceIn", CurrentPriceIn },
                    { "CurrentPriceOut", CurrentPriceOut }
                };

                var result = await _influxService.Write("house", "currentStatus", data, null, DateTime.UtcNow);
                if (!result)
                {
                    Debug.WriteLine($"Influx: {result}");
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        async void _abbPoller()
        {
            var lastSolar = DateTime.Now;
           

            while (true)
            {
                var tAbbStatus = _abbService.Get();
                
                await Task.WhenAll(tAbbStatus);
                
                var abb = await tAbbStatus;

                if (abb != null)
                {
                    lastSolar = DateTime.Now;
                    SolarOutput = abb.feeds.Feed.datastreams.m101_1_W.data[0].value;
                    SolarToday = abb.feeds.Feed.datastreams.m64061_1_DayWH.data[0].value;
                }

                if (DateTime.Now.Subtract(lastSolar) > TimeSpan.FromMinutes(20))
                {
                    //solar is stale
                    SolarOutput = 0;
                    _logService.WriteLog("Solar is stale");
                }

                _logService.WriteLog($"[{DateTime.Now.ToString()}] Power: {string.Format("{0:0.00}", SolarOutput)}");
                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }

        async void _deviceUpdates5Mins()
        {
            var lastSolar = DateTime.Now;
            var lastAmber = DateTime.Now;

            while (true)
            {
                
                var tDsStatus = _darkSkyService.GetDetail();
                var tEnergyFutures = _energyFuturesService.Get();
                await Task.WhenAll(tDsStatus, tEnergyFutures);
                CurrentWeather = await tDsStatus;
                

                EnergyFutures = await tEnergyFutures;

                if (EnergyFutures != null)
                {
                    lastAmber = DateTime.Now;
                    CurrentPriceIn = EnergyFutures.Futures[0].PriceIn;

                    CurrentPriceOut = EnergyFutures.Futures[0].PriceOut;
                    _logService.WriteLog($"Energy in: {CurrentPriceIn}");
                }

                if (DateTime.Now.Subtract(lastAmber) > TimeSpan.FromMinutes(35))
                {
                    _logService.WriteLog("Amber Data Stale");
                    CurrentPriceIn = 0;
                    CurrentPriceOut = 0;
                }

                _logService.WriteLog($"[{DateTime.Now.ToString()}] Current outside temp: {CurrentWeather.temp}");
                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }
        /// <summary>
        /// Periodically refresh device status 
        /// </summary>
        async void _deviceUpdates1Mins()
        {

            var lastDaikinSensors = DateTime.Now;
            var lastDaikinStatus = DateTime.Now;

            while (true)
            {
                var tDaikinSensors = _daikinService.GetSensors();



                var tDaikinStatus = _daikinService.GetStatus();

                await Task.WhenAll(tDaikinSensors, tDaikinStatus);


                var daikinStatus = await tDaikinStatus;
                var daikinSensors = await tDaikinSensors;




                if (daikinSensors != null)
                {
                    lastDaikinSensors = DateTime.Now;
                    DaikinInsideTemperature = Convert.ToDouble(daikinSensors["htemp"]);

                }

                if (DateTime.Now.Subtract(lastDaikinSensors) > TimeSpan.FromMinutes(5))
                {
                    //Daikin is stale
                    DaikinInsideTemperature = 0;
                    _logService.WriteLog("Daikin sensors are stale");
                }

                if (daikinStatus != null)
                {
                    lastDaikinStatus = DateTime.Now;
                    DaikinSetTemperature = Convert.ToDouble(daikinStatus?["stemp"]);
                    DaikinPoweredOn = daikinStatus?["pow"] == "1";
                    DaikinMode = daikinStatus?["mode"];
                }

                if (DateTime.Now.Subtract(lastDaikinStatus) > TimeSpan.FromMinutes(5))
                {
                    //Daikin is stale
                    DaikinSetTemperature = 0;
                    _logService.WriteLog("Daikin status is stale");
                }


                _logService.WriteLog($"[{DateTime.Now.ToString()}] inside temp: {DaikinInsideTemperature}, inside set: {DaikinSetTemperature}, power: {DaikinPoweredOn}, mode: {DaikinMode}");

                await Task.Delay(TimeSpan.FromSeconds(60));
            }
        }
    }
}
