using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EnergyHost.Services.Services
{
    public class AmberServiceV2 : IAmberServiceV2
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public AmberServiceV2(ILogService logService,
            IOptions<EnergyHostSettings> settings,
            IHttpClientFactory httpClientFactory)
        {
            _logService = logService;
            _settings = settings;
            _httpClientFactory = httpClientFactory;
        }


        public AmberPriceComposed Compose(AmberGraphDataParsed data, bool feedIn = false)
        {


            List<AmberDay> days = new List<AmberDay>();

            if (data == null || data.LivePrice == null || data.LivePrice.data == null)
                
            {
                return null;
            }

            foreach (var day in data.LivePrice.data.snapshots.billingDays)
            {
                var amberDay = new AmberDay();
                if (day.usagePeriods == null)
                {
                    continue;
                }
                foreach (var period in feedIn ?
                    day.usagePeriods.feedIn : day.usagePeriods.general)
                {
                    var p = new AmberPeriod
                    {
                        Start = period.start,
                        End = period.end,
                        Kwh = period.kwh
                    };

                    var priceFor = feedIn ?
                        data.Usage.data.snapshots.billingDays.First(_ => _.marketDate == day.marketDate)
                            .usageSummaries.feedIn.pricePeriods.First(_2 => _2.start == p.Start)
                    :
                        data.Usage.data.snapshots.billingDays.First(_ => _.marketDate == day.marketDate)
                            .usageSummaries.general.pricePeriods.First(_2 => _2.start == p.Start);

                    if (priceFor != null)
                    {                        
                        p.KwhPriceInCents = priceFor.kwhPriceInCents;
                        p.RenewablePercentage = priceFor.renewablePercentage;

                    }
                    else
                    {
                        Debug.WriteLine("Could not find amber day");
                    }

                    amberDay.Periods.Add(p);

                }
                amberDay.Start = amberDay.Periods[0].Start;
                amberDay.Kwh = amberDay.Periods.Sum(_ => _.Kwh);
                amberDay.ActualPriceInCents = amberDay.Periods.Sum(_ => _.ActualPrice);
                amberDay.usageCost = amberDay.ActualPriceInCents / 100;
                days.Add(amberDay);
            }

            var composed = new AmberPriceComposed
            {
                Days = days,
                CurrentPrice = data.LivePrice.data.sitePricing.meterWindows[0].currentPeriod.kwhPriceInCents,
                RenewablePercentage = data.LivePrice.data.sitePricing.meterWindows[0].currentPeriod.renewablePercentage
            };

            return composed;
        }

        public async Task<AmberGraphDataParsed> Get()
        {
            try
            {


                using (var client = _httpClientFactory.CreateClient())
                {
                    _logService.WriteLog($"Getting amber data: {_settings.Value.AMBER_DATA_URL}");
                    var uri = new Uri(_settings.Value.AMBER_DATA_URL);
                    var result = await client.GetAsync(uri);
                    if (!result.IsSuccessStatusCode)
                    {
                        _logService.WriteError($"Error in get amber data: {result.ReasonPhrase}");
                        return null;
                    }

                    var s = await result.Content.ReadAsStringAsync();

                    var model = JsonConvert.DeserializeObject<AmberServerResponse>(s);

                    var parsed = new AmberGraphDataParsed();

                    parsed.LivePrice = JsonConvert.DeserializeObject<AmberGraphData>(model.LivePrice);
                    parsed.Usage = JsonConvert.DeserializeObject<AmberGraphData>(model.Usage);


                    return parsed;
                }
            }
            catch (Exception ex)
            {
                _logService.WriteError(ex);
                return null;
            }

        }
    }
}
