using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.EnergyModels;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EnergyHost.Services.Services
{
    public class PowerwallService : IPowerwallService
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IInfluxService _influxService;

        public PowerwallService(ILogService logService, 
            IOptions<EnergyHostSettings> settings, 
            IHttpClientFactory httpClientFactory, 
            IInfluxService influxService)
        {
            _logService = logService;
            _settings = settings;
            _httpClientFactory = httpClientFactory;
            _influxService = influxService;
        }

        public async Task ConfigureReserve(double currentPriceIn, double batteryPercentage)
        {
            var dt = DateTime.Now;

            var setPercent = _settings.Value.TESLA_OVERNIGHT_RESERVE;

            if(setPercent == 0)
            {
                return;
            }

            if (dt.Hour >= 0 && dt.Hour < 6)
            {
                var percent = await GetReservePercent();
                
                if (percent != setPercent && batteryPercentage < setPercent)
                {
                    await SetReservePercent(setPercent);
                }
            }else if (dt.Hour == 6)
            {
                var percent = await GetReservePercent();
                if (percent != 0)
                {
                    await SetReservePercent(0);
                }
            }

        }


        public async Task<int> GetReservePercent()
        {
            try
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    var uri = new Uri(_settings.Value.TESLA_RESERVE_URL);
                    var result = await client.GetAsync(uri);
                    if (!result.IsSuccessStatusCode)
                    {
                        _logService.WriteError($"Error in get powerwall: {result.ReasonPhrase}");
                        return 0;
                    }

                    var s = await result.Content.ReadAsStringAsync();
                    return Convert.ToInt32(s);
                }
            }
            catch (Exception ex)
            {
                _logService.WriteError(ex);
                return 0;
            }
        }

        public async Task<string> SetReservePercent(int reserve)
        {
            try
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    var uri = new Uri(_settings.Value.TESLA_RESERVE_URL + $"?percent={reserve}");
                    
                    var result = await client.PutAsync(uri, null);
                    if (!result.IsSuccessStatusCode)
                    {
                        _logService.WriteError($"Error in get powerwall: {result.ReasonPhrase}");
                        return null;
                    }

                    var s = await result.Content.ReadAsStringAsync();
                    return s;
                }
            }
            catch (Exception ex)
            {
                _logService.WriteError(ex);
                return null;
            }
        }

        public async Task<double> GetUsedToday()
        {
            var dtToday = DateTime.Today.ToUniversalTime().Subtract(TimeSpan.FromMinutes(1));

            var dtMidnight = dtToday.ToString("s") + "Z";

            var start = await _influxService.Query("house", $"SELECT first(\"LoadImported\") from \"currentStatus\" WHERE time > '{dtMidnight}' tz('Australia/Sydney')");
            var end = await _influxService.Query("house", $"SELECT last(\"LoadImported\") from \"currentStatus\" WHERE time > '{dtMidnight}' tz('Australia/Sydney')");

            var startVal = start.Results[0].Series[0].Values[0][1];

            var endVal = end.Results[0].Series[0].Values[0][1];


            var result = Convert.ToDouble(endVal) - Convert.ToDouble(startVal);

            return Math.Round(result, 2);
        }

        public async Task<Powerwall> GetPowerwall()
        {
            try
            {


                using (var client = _httpClientFactory.CreateClient())
                {
                    var uri = new Uri(_settings.Value.TESLA_IP);
                    var result = await client.GetAsync(uri);
                    if (!result.IsSuccessStatusCode)
                    {
                        _logService.WriteError($"Error in get powerwall: {result.ReasonPhrase}");
                        return null;
                    }

                    var s = await result.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<Powerwall>(s);
                    return model;
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
