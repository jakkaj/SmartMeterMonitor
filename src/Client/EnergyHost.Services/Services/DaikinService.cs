using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;

namespace EnergyHost.Services.Services
{
    public class DaikinService : IDaikinService
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;

        public DaikinService(ILogService logService,
            IOptions<EnergyHostSettings> settings)
        {
            _logService = logService;
            _settings = settings;
        }

        public async Task<NameValueCollection> GetStatus()
        {
            var data = await _getDaikin("get_control_info");

            if (data == null)
            {
                return null;
            }

            var qs = HttpUtility.ParseQueryString(data.Replace(",", "&"));

            return qs;
        }

        public async Task<NameValueCollection> GetSensors()
        {
            var data = await _getDaikin("get_sensor_info");

            if (data == null)
            {
                return null;
            }

            var qs = HttpUtility.ParseQueryString(data.Replace(",", "&"));

            return qs;
        }

        async Task<string> _getDaikin(string service)
        {
            var url = $"{_settings.Value.DAIKIN_URL}/{service}?lpw={_settings.Value.DAIKIN_AUTH}";

            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(url);

                    if (!result.IsSuccessStatusCode)
                    {
                        _logService.WriteError($"Could not contact Daikin at {url}");
                        return null;
                    }

                    return await result.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    _logService.WriteError(ex.ToString());
                    return null;
                }
            }
        }
    }
}
