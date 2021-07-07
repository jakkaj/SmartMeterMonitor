using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<AmberGraphDataParsed> Get()
        {
            try
            {


                using (var client = _httpClientFactory.CreateClient())
                {
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
