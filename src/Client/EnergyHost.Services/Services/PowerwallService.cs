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

        public PowerwallService(ILogService logService, IOptions<EnergyHostSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _logService = logService;
            _settings = settings;
            _httpClientFactory = httpClientFactory;
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
