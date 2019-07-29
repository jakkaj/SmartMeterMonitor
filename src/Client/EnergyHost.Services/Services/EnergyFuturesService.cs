using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSky.Models;
using EnergyHost.Contract;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.EnergyModels;
using EnergyHost.Model.Settings;
using EnergyHost.Services.Utils;
using Microsoft.Extensions.Options;

namespace EnergyHost.Services.Services
{
    public class EnergyFuturesService : IEnergyFuturesService
    {
        private readonly ILogService _logService;
        private readonly IAmberService _amberService;
        private readonly IDarkSkyService _darkSkyService;
        private readonly IInfluxService _influxService;
        private readonly IOptions<EnergyHostSettings> _options;

        public EnergyFuturesService(
            ILogService logService, 
            IAmberService amberService, 
            IDarkSkyService darkSkyService,
            IInfluxService influxService,
            IOptions<EnergyHostSettings> options)
        {
            _logService = logService;
            _amberService = amberService;
            _darkSkyService = darkSkyService;
            _influxService = influxService;
            _options = options;
        }
        public async Task<EnergryFutures> Get()
        {
            var amberData = await _amberService.Get(_options.Value.PostCode);
            var darkData = await _darkSkyService.Get();

            var sunrise = darkData.Daily.Data[0].SunriseDateTime.Value.DateTime.AddHours(0);
            var sunset = darkData.Daily.Data[0].SunsetDateTime.Value.DateTime.AddHours(0);

            var futures = new EnergryFutures()
            {
                Futures = new List<EnergyFuture>()
            };


            var currentAd = amberData.data.variablePricesAndRenewables.Last(_ => _.periodType == "ACTUAL");
            var currentDp = darkData.Currently;

            var startTime = currentAd.period.Subtract(TimeSpan.FromDays(14)).ToUniversalTime().ConvertToISO();
            var endTime = DateTime.Now.ToUniversalTime().ConvertToISO();



            var history = await _influxService.Query("house",
                $"SELECT mean(\"SolarOutput\") FROM \"currentStatus\" WHERE time >= '{startTime}' AND time <= '{endTime}' GROUP BY time(30m)");

            var historyPairs = new List<DateDouble>();

            foreach (var i in history.Results[0].Series[0].Values)
            {
                historyPairs.Add(new DateDouble {Interval = (DateTime)i[0], Value = Convert.ToDouble(i[1])});
            }

            futures.Futures.Add(_getEnergyFuture(currentAd, currentDp, sunrise, sunset, false, historyPairs));


            foreach (var ad in amberData.data.variablePricesAndRenewables.Where(_=>_.periodType == "FORECAST"))
            {
                var ds = _pair(ad, darkData);
                futures.Futures.Add(_getEnergyFuture(ad, ds, sunrise, sunset, true, historyPairs));
            }

            return futures;
        }

        EnergyFuture _getEnergyFuture(VariablePricesAndRenewable amberVars, DataPoint dp, DateTime sunrise, DateTime sunset, bool isForecast, List<DateDouble> history)
        {
            
            var obj = new EnergyFuture()
            {
                AmberPrices = amberVars,
                Cloudiness = dp.CloudCover ?? 1,                
                Temperature = dp.Temperature ?? 0,
                DarkSkyDataPoint = dp,                
                PrecipIntensity = dp.PrecipIntensity ?? 0,
                PrecipProbability = dp.PrecipProbability ?? 0,
                Period = amberVars.period,
                PriceIn = amberVars.InPrice,
                PriceInNormalised = amberVars.InPriceNormal,
                PriceOut = amberVars.OutPrice,
                PriceOutNormalised = amberVars.OutPriceNormal,
                IsForecast = isForecast,                
            };           
           
            var historyRange = history.Where(e => e.Interval.Hour == amberVars.period.ToUniversalTime().Hour && e.Interval.Minute == amberVars.period.ToUniversalTime().Minute).Select(e=>e.Value).ToList();

            var aggregate = historyRange.Average();

            obj.SolarHistory = aggregate;
   
            var solarNormalised = _normalise(aggregate, 0, 3);
            
            obj.Value = _normalise(solarNormalised - obj.Cloudiness - obj.PriceInNormalised, 0, 3);

            obj.SolarValue = solarNormalised - (obj.Cloudiness / 4);
           
            if(obj.SolarValue < 0)
                obj.SolarValue = 0;
                
            obj.GridValue = (1 - obj.PriceInNormalised);

            obj.UsageSuggestion = obj.GridValue + obj.SolarValue;
            obj.CostSuggestion = 2 - obj.UsageSuggestion;

            return obj;

        }

        private double _normalise(double value, double min, double max) {
	            var normalised = (value - min) / (max - min);
	            return normalised;
        }

        DataPoint _pair(VariablePricesAndRenewable amberVars, Forecast dp)
        {
            var t = amberVars.period;

            var ds = dp.Hourly.Data.OrderByDescending(_ => _.DateTime).FirstOrDefault(_ => _.DateTime <= t);

            return ds;

        }

        
    }
}
