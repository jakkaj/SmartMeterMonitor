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

            var sunrise = darkData.Daily.Data[0].SunriseDateTime.Value.DateTime.AddHours(.5);
            var sunset = darkData.Daily.Data[0].SunsetDateTime.Value.DateTime.AddHours(.5);

            var futures = new EnergryFutures()
            {
                Futures = new List<EnergyFuture>()
            };


            var currentAd = amberData.data.variablePricesAndRenewables.Last(_ => _.periodType == "ACTUAL");
            var currentDp = darkData.Currently;

            futures.Futures.Add(_getEnergyFuture(currentAd, currentDp, sunrise, sunset));


            foreach (var ad in amberData.data.variablePricesAndRenewables.Where(_=>_.periodType == "FORECAST"))
            {
                var ds = _pair(ad, darkData);
                futures.Futures.Add(_getEnergyFuture(ad, ds, sunrise, sunset));
            }

            return futures;
        }

        EnergyFuture _getEnergyFuture(VariablePricesAndRenewable amberVars, DataPoint dp, DateTime sunrise, DateTime sunset)
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
                PriceOutNormalised = amberVars.OutPriceNormal
            };

            var isLight = amberVars.period.Hour > sunrise.Hour && amberVars.period.Hour < sunset.Hour;

            obj.SolarBad = isLight ? obj.Cloudiness : 1;

            obj.Value = (2 - obj.SolarBad - obj.PriceInNormalised) / 2;

            obj.SolarValue = (1 - obj.SolarBad);
            obj.GridValue = (1 - obj.PriceInNormalised);

            return obj;

        }

        DataPoint _pair(VariablePricesAndRenewable amberVars, Forecast dp)
        {
            var t = amberVars.period;

            var ds = dp.Hourly.Data.OrderByDescending(_ => _.DateTime).FirstOrDefault(_ => _.DateTime <= t);

            return ds;

        }

        
    }
}
