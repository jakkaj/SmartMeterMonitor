using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSky.Models;
using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;

namespace EnergyHost.Services.Services
{
    public class EnergyFuturesService : IEnergyFuturesService
    {
        private readonly ILogService _logService;
        private readonly IAmberService _amberService;
        private readonly IDarkSkyService _darkSkyService;
        private readonly IOptions<EnergyHostSettings> _options;

        public EnergyFuturesService(
            ILogService logService, 
            IAmberService amberService, 
            IDarkSkyService darkSkyService,
            IOptions<EnergyHostSettings> options)
        {
            _logService = logService;
            _amberService = amberService;
            _darkSkyService = darkSkyService;
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

            futures.Futures.Add(_getEnergyFuture(currentAd, currentDp, sunrise, sunset, false));


            foreach (var ad in amberData.data.variablePricesAndRenewables.Where(_=>_.periodType == "FORECAST"))
            {
                var ds = _pair(ad, darkData);
                futures.Futures.Add(_getEnergyFuture(ad, ds, sunrise, sunset, true));
            }

            return futures;
        }

        EnergyFuture _getEnergyFuture(VariablePricesAndRenewable amberVars, DataPoint dp, DateTime sunrise, DateTime sunset, bool isForecast)
        {
            
            var obj = new EnergyFuture()
            {
                AmberPrices = amberVars,
                Cloudiness = dp.CloudCover ?? 1,
                DarkSkyDataPoint = dp,
                Period = amberVars.period,
                PriceIn = amberVars.InPrice,
                PriceInNormalised = amberVars.InPriceNormal,
                PriceOut = amberVars.OutPrice,
                PriceOutNormalised = amberVars.OutPriceNormal,
                IsForecast = isForecast,
                
            };

            var isLight = amberVars.period.Hour > sunrise.Hour && amberVars.period.Hour < sunset.Hour;

            var timeFromSunrise = amberVars.period.Hour - sunrise.Hour;
            var timeFromSunset = sunset.Hour - amberVars.period.Hour;

            var solarHigh = 0d;

            if(isLight){
                var nowTicks = amberVars.period.Ticks;
                var halfTicks = sunset.Ticks - (sunset.Ticks - sunrise.Ticks) / 2;

                //find if time is closer to sunset or sunrise
                if (nowTicks > halfTicks)
                {
                    //closer to sunset so normal from here
                    solarHigh = _normalise(nowTicks, halfTicks, sunset.Ticks);
                    solarHigh = 1 - solarHigh;
                }
                else{
                    //closer to sunrise
                    solarHigh = _normalise(nowTicks, sunrise.Ticks, halfTicks);
                    
                }
            }

            obj.SolarBad = isLight ? obj.Cloudiness : 1;
            obj.SolarBad = isLight ? obj.SolarBad - solarHigh : 1;
            obj.Value = (2 - obj.SolarBad - obj.PriceInNormalised) / 2;

            obj.SolarValue = (1 - obj.SolarBad);
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
