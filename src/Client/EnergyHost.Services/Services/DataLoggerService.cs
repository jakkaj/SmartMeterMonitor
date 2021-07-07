using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.EnergyModels;
using EnergyHost.Model.EnergyModels.Status;
using EnergyHost.Services.Contract;
using EnergyHost.Services.Utils;

namespace EnergyHost.Services.Services
{
    public class DataLoggerService : IDataLoggerService
    {
        private readonly ILogService _logService;
        private readonly ISunSpecService _abbService;
        private readonly IDarkSkyService _darkSkyService;
        private readonly IInfluxService _influxService;
        private readonly IAmberServiceV2 _amberService;
        private readonly IEnergyFuturesService _energyFuturesService;
        private readonly IMQTTService _mqttService;
        private readonly ISystemStatusService _statusService;
        private readonly IThresholdingService _thresholdingService;
        private readonly INotificationService _notificationService;
        private readonly INetatmoService _netatmoService;
        private readonly IPowerwallService _powerwallService;
        private readonly IDaikinService _daikinService;

        public double SolarOutput { get; set; } = 0;
        public double EnergyUsage { get; set; } = 0; //Grid
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
        public NetatmoData NetatmoData { get; set; }
        public double CurrentPriceIn { get; set; }
        public double NextPriceIn { get; set; }
        public double CurrentPriceOut { get; set; }
        public double NextPriceOut { get; set; }
        public double FeedIn { get; set; }
        public double SelfConsumption { get; set; }
        public double Purchased { get; set; }
        public double Consumption { get; set; }
        public AmberGraphDataParsed AmberUsage { get; set; }

        
        public double BatteryUsage { get; set; } //Battery
        public double LoadUsage { get; set; } //House
        public double BatteryLevel { get; set; }
        public bool IsCharging { get; set; }
        public bool IsDischarging { get; set; }
        public double LoadImported { get; set; }
        public double SolarExported { get; set; }
        public double BatteryImported { get; set; }
        public double BatteryExported { get; set; }



        public DataLoggerService(ILogService logService,
            IDaikinService daikinService,
            ISunSpecService abbService,
            IDarkSkyService darkSkyService,
            IInfluxService influxService,
            IAmberServiceV2 amberService,
            IEnergyFuturesService energyFuturesService,
            IMQTTService mqttService,
            ISystemStatusService statusService,
            IThresholdingService thresholdingService,
            INotificationService notificationService,
            INetatmoService netatmoService,
            IPowerwallService powerwallService
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
            _netatmoService = netatmoService;
            _powerwallService = powerwallService;
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
                    { "powerTotal", LoadUsage},
                    { "temp", NetatmoData?.OutdoorTemp ?? 0},
                    { "indoorTemp", NetatmoData?.IndoorTemp ?? 0 },
                    { "humidity", NetatmoData?.OutdoorHumidity ?? 0},
                    { "indoorHumidity", NetatmoData?.IndoorHumidity ?? 0},
                    { "pressure", NetatmoData?.Pressure ?? 0},
                    { "co2", NetatmoData?.CO2 ?? 0},
                    { "rain", NetatmoData?.Rain ?? 0},
                    { "rain24", NetatmoData?.Rain24 ?? 0},
                    { "rain1", NetatmoData?.Rain1 ?? 0},
                    { "noise", NetatmoData?.Noise ?? 0},
                    { "windSpeed", Convert.ToDouble(NetatmoData?.WindStrength ?? 0)},
                    { "windGusts", Convert.ToDouble(NetatmoData?.WindGusts ?? 0)},
                    { "windAngle", Convert.ToDouble(NetatmoData?.WindAngle ?? 0)},
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
                    { "SolarHistory", EnergyFutures?.Futures[0].SolarHistory != null ? EnergyFutures?.Futures[0].SolarHistory : 0},
                    { "CurrentPriceIn", CurrentPriceIn },
                    { "CurrentPriceOut", CurrentPriceOut },
                    { "NextPriceIn", NextPriceIn },
                    { "BatteryUsage", BatteryUsage },
                    { "BatteryLevel", BatteryLevel },
                    { "IsCharging", IsCharging },
                    { "IsDischarging", IsDischarging },
                    { "LoadImported", LoadImported },
                    { "SolarExported", SolarExported },
                    { "BatteryImported", BatteryImported },
                    { "BatteryExported", BatteryExported }


                    //{ "MonthTotalCost", AmberUsage?.data.lastMonthUsage.FromGrid.actualCost ?? 0 },
                    //{ "LastWeekTotalCost", AmberUsage?.data.lastWeekUsage.FromGrid.actualCost ?? 0},
                    //{ "WeekTotalCost", AmberUsage?.data.thisWeekUsage?.FromGrid?.actualCost ?? 0},
                    //{ "MonthTotalSolarCost", AmberUsage?.data.lastMonthUsage.ToGrid.totalUsageCostInCertainPeriod ?? 0 },
                    //{ "LastWeekTotalSolarCost", AmberUsage?.data.lastWeekUsage.ToGrid.totalUsageCostInCertainPeriod ?? 0},
                    //{ "WeekSolarCost", AmberUsage?.data.thisWeekUsage?.ToGrid?.totalUsageCostInCertainPeriod ?? 0},
                    //{ "YesterdayTotalCost", AmberUsage?.data.thisWeekDailyUsage.Where(_=>_.meterSuffix=="E1").OrderByDescending(_=>_.date).FirstOrDefault()?.actualCost ?? 0},
                    //{ "YesterdaySolarCost", AmberUsage?.data.thisWeekDailyUsage.Where(_=>_.meterSuffix=="B1").OrderByDescending(_=>_.date).FirstOrDefault()?.usageCost ?? 0},


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
            await _writeUsageV2(_amberService.Compose(AmberUsage), "FromGrid");
            await _writeUsageV2(_amberService.Compose(AmberUsage, true), "ToGrid");
        }



        public async Task _writeUsageV2(AmberPriceComposed composed, string meter)
        {
            foreach(var d in composed.Days)
            {                
                await _influxService.WriteObject("house", $"amberDailyUsage{meter}", d, null, d.Start.ToUniversalTime());
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
                await _powerwallService.ConfigureReserve(CurrentPriceIn, BatteryLevel);

                AmberUsage = await _amberService.Get();
                

                if (AmberUsage != null)
                {
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
                var tNetatmoData = _netatmoService.Get();
                await Task.WhenAll(tEnergyFutures, tNetatmoData);
                EnergyFutures = await tEnergyFutures;
                NetatmoData = await tNetatmoData;

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
                var tPowerall = _powerwallService.GetPowerwall();
                var tUsed = _powerwallService.GetUsedToday();
                await Task.WhenAll(tAbbModbus, tPowerall, tUsed);

                var abbModbus = await tAbbModbus;

                var powerWall = await tPowerall;

                var pUsed = await tUsed;

                if (powerWall != null)
                {
                    EnergyUsage = powerWall.site.instant_power / 1000;
                    BatteryUsage = powerWall.battery.instant_power / 1000;
                    LoadUsage = powerWall.load.instant_power / 1000;
                    BatteryLevel = powerWall.charge;
                    IsCharging = powerWall.battery.instant_power < 0;
                    IsDischarging = powerWall.battery.instant_power > 0;
                    
                    LoadImported = Math.Round(powerWall.load.energy_imported / 1000, 2);
                    SolarExported = Math.Round(powerWall.solar.energy_exported / 1000, 2);
                    BatteryExported = Math.Round(powerWall.battery.energy_exported / 1000, 2);
                    BatteryImported = Math.Round(powerWall.battery.energy_imported / 1000, 2);

                    Consumption = pUsed;
                }

                if (abbModbus != null)
                {

                    //if (-abbModbus?.meter?.W != null && -abbModbus.meter.W != 0)
                    //{
                    //    EnergyUsage = -abbModbus.meter.W / 1000;
                    //}
                    ////else
                    ////{
                    ////    EnergyUsage = abbModbus.siteCurrentPowerFlow.GRID.currentPower;
                    ////}

                    if (abbModbus.W != 0)
                    {
                        SolarOutput = Convert.ToDouble(abbModbus.W) / 1000;
                    }

                    if (abbModbus?.energyDetails != null)
                    {
                        SolarToday = Convert.ToDouble(abbModbus.energyDetails.meters.First(_ => _.type == "Production").values[0].value) / 1000;
                        //Consumption = Convert.ToDouble(abbModbus.energyDetails.meters.First(_ => _.type == "Consumption").values[0].value) / 1000;
                        Purchased = Convert.ToDouble(abbModbus.energyDetails.meters.First(_ => _.type == "Purchased").values[0].value) / 1000;
                        SelfConsumption = Convert.ToDouble(abbModbus.energyDetails.meters.First(_ => _.type == "SelfConsumption").values[0].value) / 1000;
                        FeedIn = Convert.ToDouble(abbModbus.energyDetails.meters.First(_ => _.type == "FeedIn").values[0].value) / 1000;

                    }
                    else
                    {
                        SolarOutput = 0;
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
        async void _daikinUpdate()
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
