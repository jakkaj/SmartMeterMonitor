using EnergyHost.Contract;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.EnergyModels;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EnergyHost.Services.Services
{
    public class ClipsalService : IClipsalService
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;
        private readonly IHttpClientFactory _httpClientFactory;
        public ClipsalService(ILogService logService,
            IOptions<EnergyHostSettings> settings,
            IHttpClientFactory httpClientFactory)
        {
            _logService = logService;
            _settings = settings;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<List<ClipsalUsage>> Get()
        {
            try
            {


                using (var client = _httpClientFactory.CreateClient())
                {
                    _logService.WriteLog($"Getting Clipsal data: {_settings.Value.CLIPSAL_DATA_URL}");
                    var uri = new Uri(_settings.Value.CLIPSAL_DATA_URL);
                    var result = await client.GetAsync(uri);
                    if (!result.IsSuccessStatusCode)
                    {
                        _logService.WriteError($"Error in get clipsa; data: {result.ReasonPhrase}");
                        return null;
                    }

                    var s = await result.Content.ReadAsStringAsync();

                    var model = JsonConvert.DeserializeObject<AmberServerResponse>(s);

                    var parsed = JsonConvert.DeserializeObject<List<ClipsalUsage>>(model.Usage);

                    

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
