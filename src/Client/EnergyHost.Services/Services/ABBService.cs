using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EnergyHost.Services.Services
{
    public class ABBService : IABBService
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ABBService(ILogService logService, IOptions<EnergyHostSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _logService = logService;
            _settings = settings;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ABBSunspec> GetModbus()
        {
            try{

            
            using (var client = _httpClientFactory.CreateClient())
            {
                var uri = new Uri(_settings.Value.ABB_MODBUS_URL);
                var result = await client.GetAsync(uri);
                if (!result.IsSuccessStatusCode)
                {
                    _logService.WriteError($"Error in get modbus: {result.ReasonPhrase}");
                    return null;
                }
                var model = JsonConvert.DeserializeObject<ABBSunspec>(await result.Content.ReadAsStringAsync());
                return model;
            }
            }catch(Exception ex)
            {
                _logService.WriteError(ex);
                return null;
            }
        }

        public async Task<ABBDevice> Get()
        {
            var retryCount = 0;

            while (retryCount < 3)
            {

                using (var client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("X-Digest", _settings.Value.ABB_AUTH);

                    var url = _settings.Value.ABB_URL;
                    try
                    {
                        var result = await client.GetAsync(new Uri(url));

                        if (!result.IsSuccessStatusCode)
                        {
                            return null;
                        }

                        var stringResult = await result.Content.ReadAsStringAsync();
                        var abbData = JsonConvert.DeserializeObject<ABBDevice>(stringResult);
                        return abbData;
                    }
                    catch (Exception ex)
                    {
                        _logService.WriteError(ex.ToString());
                        
                    }

                    retryCount++;
                    Debug.WriteLine($"ABB Retry: {retryCount}");

                    
                }

            }

            return null;


        }
    }
}
