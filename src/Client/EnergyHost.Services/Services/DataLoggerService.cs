using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels;
using EnergyHost.Model.EnergyModels.Status;
using EnergyHost.Services.Utils;

namespace EnergyHost.Services.Services
{
    public class DataLoggerService : IDataLoggerService
    {
        private readonly ILogService _logService;
        private readonly ISunSpecService _abbService;
        private readonly IDarkSkyService _darkSkyService;
        private readonly IInfluxService _influxService;
        private readonly IAmberService _amberService;
        private readonly IEnergyFuturesService _energyFuturesService;
        private readonly IMQTTService _mqttService;
        private readonly ISystemStatusService _statusService;
        private readonly IThresholdingService _thresholdingService;
        private readonly INotificationService _notificationService;
        private readonly IDaikinService _daikinService;

        public double SolarOutput { get; set; } = 0;
        public double EnergyUsage { get; set; } = 0;
        public double SystemVoltage { get; set; } = 0;
        public double SolarToday { get; set; } = 0;
        public double DaikinInsideTemperature { get; set; }
        public double DaikinSetTemperature { get; set; }
        public string DaikinMode { get; set; }
        public bool DaikinPoweredOn { get; set; }
        public (double temp, double humid, double pressure,
            double wind, double minToday, double maxToday,
            double minTomorrow, double maxTomorrow, double cloudiness, double dewpoint) CurrentWeather
        { get; set; }

        public EnergryFutures EnergyFutures { get; set; }
        public double CurrentPriceIn { get; set; }
        public double NextPriceIn { get; set; }
        public double CurrentPriceOut { get; set; }
        public double NextPriceOut { get; set; }
        public double FeedIn { get; set; }
        public double SelfConsumption { get; set; }
        public double Purchased { get; set; }
        public double Consumption { get; set; }
        public AmberUsage AmberUsage { get; set; }

        public DataLoggerService(ILogService logService,
            IDaikinService daikinService,
            ISunSpecService abbService,
            IDarkSkyService darkSkyService,
            IInfluxService influxService,
            IAmberService amberService,
            IEnergyFuturesService energyFuturesService,
            IMQTTService mqttService,
            ISystemStatusService statusService,
            IThresholdingService thresholdingService,
            INotificationService notificationService
            )
        {
            _logService = logService;
            _abbService = abbService;
            _darkSkyService = darkSkyService;
            _influxService = influxService;
            _amberService = amberService;
            _energyFuturesService = energyFuturesService;
            _mqttService = mqttService;
            _statusService = statusService;
            _thresholdingService = thresholdingService;
            _notificationService = notificationService;
            _daikinService = daikinService;
        }


        public async Task Start()
        {
            _daikinUpdate();

            _deviceUpdates10Mins();

            //_abbPoller();

            _deviceUpdate10s();

            _fiveMinuteEvenPoller();
            _hourEvenPoller();

            await _mqttService.Setup();

            await Task.Delay(TimeSpan.FromSeconds(10));

            _ = Task.Run(async () =>
              {
                  while (true)
                  {
                      _dbWriter();
                      await Task.Delay(TimeSpan.FromSeconds(10));
                  }
              });
        }

        async void _dbWriter()
        {

            var data = new Dictionary<string, object>
                {
                    { "kwh", EnergyUsage },
                    { "ctkwh", EnergyUsage},
                    { "powerTotal", EnergyUsage + SolarOutput},
                    { "temp", CurrentWeather.temp},
                    { "temp3", Convert.ToDouble(_mqttService.Values["temp1"])},
                    { "humid3", Convert.ToDouble(_mqttService.Values["humid1"])},
                    { "humidity", CurrentWeather.humid},
                    { "pressure", CurrentWeather.pressure},
                    { "windSpeed", CurrentWeather.wind},
                    { "cloudiness", CurrentWeather.cloudiness},
                    { "min", CurrentWeather.minToday},
                    { "max", CurrentWeather.maxToday},
                    { "minTomorrow", CurrentWeather.minTomorrow},
                    { "maxTomorrow", CurrentWeather.maxTomorrow},
                    { "dewPoint", CurrentWeather.dewpoint },
                    { "DaikinSetTemperature", DaikinSetTemperature },
                    { "DaikinInsideTemperature", DaikinInsideTemperature },
                    { "DaikinMode", DaikinMode },
                    { "DaikinPoweredOn", DaikinPoweredOn },
                    { "SolarOutput", SolarOutput },
                    { "SolarToday", SolarToday},
                    { "Purchased", Purchased },
                    { "SelfConsumption", SelfConsumption },
                    { "FeedIn", FeedIn },
                    { "Consumption", Consumption },
                    { "SystemVoltage", SystemVoltage },
                    { "SolarHistory", EnergyFutures.Futures[0].SolarHistory },
                    { "CurrentPriceIn", CurrentPriceIn },
                    { "CurrentPriceOut", CurrentPriceOut },
                    { "NextPriceIn", NextPriceIn },
                    { "NextPriceOut", NextPriceOut },
                    { "MonthTotalCost", AmberUsage?.data.lastMonthUsage.FromGrid.actualCost ?? 0 },
                    { "LastWeekTotalCost", AmberUsage?.data.lastWeekUsage.FromGrid.actualCost ?? 0},
                    { "WeekTotalCost", AmberUsage?.data.thisWeekUsage.FromGrid.actualCost ?? 0},
                    { "MonthTotalSolarCost", AmberUsage?.data.lastMonthUsage.ToGrid.totalUsageCostInCertainPeriod ?? 0 },
                    { "LastWeekTotalSolarCost", AmberUsage?.data.lastWeekUsage.ToGrid.totalUsageCostInCertainPeriod ?? 0},
                    { "WeekSolarCost", AmberUsage?.data.thisWeekUsage.ToGrid.totalUsageCostInCertainPeriod ?? 0},
                    { "YesterdayTotalCost", AmberUsage?.data.thisWeekDailyUsage.Where(_=>_.meterSuffix=="E1").OrderByDescending(_=>_.date).First().actualCost ?? 0},
                    { "YesterdaySolarCost", AmberUsage?.data.thisWeekDailyUsage.Where(_=>_.meterSuffix=="B1").OrderByDescending(_=>_.date).First().usageCost ?? 0},


                };


            if (!Debugger.IsAttached)
            {
                var result = await _influxService.Write("house", "currentStatus", data, null, null);
                if (!result)
                {
                    _logService.WriteError($"Influx fail save");
                    Debug.WriteLine($"Influx: {result}");
                }

            }

            await _writeFutures();



            await _statusService.SendStatus(new EnergyPriceStatus
            {
                CurrentPriceIn = CurrentPriceIn,
                CurrentPriceOut = CurrentPriceOut

            });

            await _statusService.SendStatus(new DaikinStatus
            {
                DaikinSetTemperature = DaikinSetTemperature,
                DaikinInsideTemperature = DaikinInsideTemperature,
                DaikinMode = DaikinMode,
                DaikinPoweredOn = DaikinPoweredOn
            });

            await _statusService.SendStatus(new WeatherStatus
            {
                CurrentTemp = CurrentWeather.temp,
                Humidity = CurrentWeather.humid,
                Pressure = CurrentWeather.pressure,
                WindSpeed = CurrentWeather.wind,
                MinToday = CurrentWeather.minToday,
                MaxToday = CurrentWeather.maxToday,
                MinTomorrow = CurrentWeather.minTomorrow,
                MaxTomorrow = CurrentWeather.maxTomorrow

            });

            await _statusService.SendStatus(new PowerStatus
            {
                KWHIn = _mqttService.KWH,
                KWHSolar = SolarOutput,
                KWHSolarToday = SolarToday
            });

            await _statusService.SendStatus(new TimeStatus(), false);//time pump

            await _thresholdingService.RunChecks(data);

        }

        public async Task _writeAmberUsage()
        {
            while (AmberUsage == null)
            {
                await Task.Delay(5000);
            }

            var usage = new List<DailyUsage>();
            usage.AddRange(AmberUsage.data.lastMonthDailyUsage);
            usage.AddRange(AmberUsage.data.lastWeekDailyUsage);
            usage.AddRange(AmberUsage.data.thisWeekDailyUsage);

            await _writeUsage(usage);


            await _influxService.WriteObject("house", "amberPeriodUsageFromGrid", AmberUsage.data.lastMonthUsage, null,
                AmberUsage.data.lastMonthUsage.FromGrid.date);
            await _influxService.WriteObject("house", "amberPeriodUsageToGrid", AmberUsage.data.lastMonthUsage, null,
                AmberUsage.data.lastMonthUsage.ToGrid.date);

            await _influxService.WriteObject("house", "amberPeriodUsageFromGrid", AmberUsage.data.lastWeekUsage, null,
                AmberUsage.data.lastWeekUsage.FromGrid.date);
            await _influxService.WriteObject("house", "amberPeriodUsageToGrid", AmberUsage.data.lastWeekUsage, null,
                AmberUsage.data.lastWeekUsage.ToGrid.date);

            if (AmberUsage.data.thisWeekUsage.ToGrid != null) { 
                await _influxService.WriteObject("house", "amberPeriodUsageFromGrid", AmberUsage.data.thisWeekUsage,
                    null, AmberUsage.data.thisWeekUsage.FromGrid.date);
            await _influxService.WriteObject("house", "amberPeriodUsageToGrid", AmberUsage.data.thisWeekUsage, null,
                AmberUsage.data.thisWeekUsage.ToGrid.date);
            }
        }



        public async Task _writeUsage(List<DailyUsage> usage)
        {
            foreach (var u in usage)
            {
                u.date = u.date.ToUniversalTime();
                var meterSuffix = u.meterSuffix == "B1" ? "ToGrid" : "FromGrid";

                await _influxService.WriteObject("house", $"amberDailyUsage{meterSuffix}", u, null, u.date);

            }

        }

        public async Task _writeFutures()
        {
            if (EnergyFutures == null)
            {
                return;
            }

            foreach (var i in EnergyFutures.Futures)
            {
                //var utcNow = DateTime.UtcNow;
                var t = i.IsForecast ? i.Period.ToUniversalTime() : DateTime.Now.ToUniversalTime();
                await _influxService.WriteObject("house", "energyFutures", i, null, t);
            }
        }

        async void _hourEvenPoller()
        {
            while (true)
            {
                AmberUsage = await _amberService.GetUsage();
                if(AmberUsage!=null){
                    await _writeAmberUsage();
                }
                

                while (true)
                {
                    if (DateTime.Now.Minute == 0)
                    {
                        break;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }

                await Task.Delay(TimeSpan.FromSeconds(130));
            }
        }

        async void _fiveMinuteEvenPoller()
        {
            var lastAmber = DateTime.Now;

            while (true)
            {
                var tEnergyFutures = _energyFuturesService.Get();
                await Task.WhenAll(tEnergyFutures);
                EnergyFutures = await tEnergyFutures;
                if (EnergyFutures != null)
                {
                    lastAmber = DateTime.Now;

                    if (EnergyFutures.Futures.Count > 1)
                    {
                        CurrentPriceIn = EnergyFutures.Futures[0].PriceIn;
                        NextPriceIn = EnergyFutures.Futures[1].PriceIn;


                        CurrentPriceOut = EnergyFutures.Futures[0].PriceOut;
                        NextPriceOut = EnergyFutures.Futures[1].PriceOut;
                        _logService.WriteLog($"Energy in: {CurrentPriceIn}");

                        await _notificationService.SendPrice(CurrentPriceIn);
                    }
                    else
                    {
                        _logService.WriteError("No Energy Future Data!");
                    }

                }

                while (true)
                {
                    if (DateTime.Now.Minute % 5 == 0 && DateTime.Now.Second == 00)
                    {
                        break;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(.5));
                }

                await Task.Delay(TimeSpan.FromSeconds(130));

                if (DateTime.Now.Subtract(lastAmber) > TimeSpan.FromMinutes(35))
                {
                    _logService.WriteLog("Amber Data Stale");
                    CurrentPriceIn = 0;
                    CurrentPriceOut = 0;
                    NextPriceIn = 0;
                    NextPriceOut = 0;
                }

            }
        }

        //async void _abbPoller()
        //{
        //    var lastSolar = DateTime.Now;


        //    while (true)
        //    {
        //        var tAbbStatus = _abbService.Get();

        //        await Task.WhenAll(tAbbStatus);

        //        var abb = await tAbbStatus;

        //        if (abb != null)
        //        {
        //            lastSolar = DateTime.Now;
        //            //
        //            SolarToday = abb.feeds.Feed.datastreams.m64061_1_DayWH.data[0].value;
        //        }

        //        if (DateTime.Now.Subtract(lastSolar) > TimeSpan.FromMinutes(20))
        //        {
        //            //solar is stale
        //            SolarOutput = 0;
        //            _logService.WriteLog("Solar is stale");
        //        }

        //        _logService.WriteLog($"[{DateTime.Now.ToString()}] Power: {string.Format("{0:0.00}", SolarOutput)}");
        //        await Task.Delay(TimeSpan.FromMinutes(5));
        //    }
        //}

        async void _deviceUpdate10s()
        {
            while (true)
            {
                var tAbbModbus = _abbService.GetModbus();

                await Task.WhenAll(tAbbModbus);

                var abbModbus = await tAbbModbus;

                if (abbModbus != null)
                {

                    EnergyUsage = -abbModbus.meter.W / 1000;
                    SolarOutput = Convert.ToDouble(abbModbus.W) / 1000;
                    if (abbModbus?.energyDetails != null)
                    {
                        SolarToday = Convert.ToDouble(abbModbus.energyDetails.meters.First(_ => _.type == "Production").values[0].value) / 1000;
                        Consumption = Convert.ToDouble(abbModbus.energyDetails.meters.First(_ => _.type == "Consumption").values[0].value) / 1000;
                        Purchased = Convert.ToDouble(abbModbus.energyDetails.meters.First(_ => _.type == "Purchased").values[0].value) / 1000;
                        SelfConsumption = Convert.ToDouble(abbModbus.energyDetails.meters.First(_ => _.type == "SelfConsumption").values[0].value) / 1000;
                        FeedIn = Convert.ToDouble(abbModbus.energyDetails.meters.First(_ => _.type == "FeedIn").values[0].value) / 1000;

                    }

                    if (abbModbus.PhVphA != null)
                    {
                        SystemVoltage = (double)abbModbus.PhVphA;
                    }
                    else
                    {
                        SystemVoltage = 0;
                    }

                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        async void _deviceUpdates10Mins()
        {
            var lastSolar = DateTime.Now;


            while (true)
            {

                var tDsStatus = _darkSkyService.GetDetail();

                await Task.WhenAll(tDsStatus);
                CurrentWeather = await tDsStatus;

                //_logService.WriteLog($"[{DateTime.Now.ToString()}] Current outside temp: {CurrentWeather.temp}");

                await Task.Delay(TimeSpan.FromMinutes(10));
            }




        }
        /// <summary>
        /// Periodically refresh device status 
        /// </summary>
        async void _daikinUpdate ()
        {

            var lastDaikinSensors = DateTime.Now;
            var lastDaikinStatus = DateTime.Now;

            while (true)
            {
                var tDaikinSensors = _daikinService.GetSensors();



                var tDaikinStatus = _daikinService.GetControlInfo();


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
                    DaikinSetTemperature = Convert.ToDouble(daikinStatus?.stemp);
                    DaikinPoweredOn = daikinStatus?.pow == "1";
                    DaikinMode = daikinStatus?.mode;
                }

                if (DateTime.Now.Subtract(lastDaikinStatus) > TimeSpan.FromMinutes(5))
                {
                    //Daikin is stale
                    DaikinSetTemperature = 0;
                    _logService.WriteLog("Daikin status is stale");
                }


                _logService.WriteLog($"[{DateTime.Now.ToString()}] inside temp: {DaikinInsideTemperature}, inside set: {DaikinSetTemperature}, power: {DaikinPoweredOn}, mode: {DaikinMode}");

                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }
    }
}
