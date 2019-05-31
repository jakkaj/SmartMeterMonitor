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

            var futures = new EnergryFutures()
            {
                Futures = new List<EnergyFuture>()
            };


            var currentAd = amberData.data.variablePricesAndRenewables.Last(_ => _.periodType == "ACTUAL");
            var currentDp = darkData.Currently;

            futures.Futures.Add(_getEnergyFuture(currentAd, currentDp));


            foreach (var ad in amberData.data.variablePricesAndRenewables.Where(_=>_.periodType == "FORECAST"))
            {
                var ds = _pair(ad, darkData);
                futures.Futures.Add(_getEnergyFuture(ad, ds));
            }

            return futures;
        }

        EnergyFuture _getEnergyFuture(VariablePricesAndRenewable amberVars, DataPoint dp)
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

            obj.SolarBad = dp.UvIndex != 0 ? obj.Cloudiness : 1;

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
