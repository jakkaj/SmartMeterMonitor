using EnergyHost.Contract;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.EnergyModels;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        bool _lastOvenOn = false;
        DateTime _startOven = DateTime.Now;

        public async Task CheckOven(double oven)
        {
            var ovenOn = oven > 1;

            if (ovenOn && !_lastOvenOn)
            {
                _startOven = DateTime.Now;
            }

            var ovenWarmUp = TimeSpan.FromMinutes(6);

            if (!ovenOn && _lastOvenOn && DateTime.Now.Subtract(_startOven) > ovenWarmUp)
            {
                //call the url
                await _notifyOven();                
            }

            _lastOvenOn = ovenOn;




        }

        async Task _notifyOven()
        {
            try
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    _logService.WriteLog($"Notify Oven: {_settings.Value.CLIPSAL_OVEN_URL}");

                    var uri = new Uri(_settings.Value.CLIPSAL_OVEN_URL); //using www.virtualbuttons.com
                    var result = await client.GetAsync(uri);
                }
            }
            catch (Exception ex)
            {
                _logService.WriteError(ex);
                return;
            }

        }

        public List<ClipsalInflux> Compose(List<ClipsalUsage> usage)
        {
            var l = new List<ClipsalInflux>();
            foreach (var u in usage)
            {
                var ci = new ClipsalInflux
                {
                    date = DateTime.Parse(u.datetime).ToUniversalTime(),
                    powerpoints = (u.load_powerpoint__1 ?? 0) / 1000,
                    oven = (u.load_oven__1 ?? 0) / 1000,
                    ac = (u.load_air_conditioner__1 ?? 0) / 1000,
                    other = (u.load_residual ?? 0) / 1000
                };
                l.Add(ci);
            }

            return l;
        }

        public async Task<List<ClipsalUsage>> Get(int days)
        {
            var end = DateTime.Now.Subtract(TimeSpan.FromDays(days));

            var start = end.Subtract(TimeSpan.FromDays(1));

            var qs = $"from_datetime={_getDateString(start)}&to_datetime={_getDateString(end)}";

            return await _get(qs);
        }

        string _getDateString(DateTime dt)
        {
            //from_datetime=2021-07-07 00:00:00&to_datetime=2021-07-08 00:00:00
            return System.Web.HttpUtility.UrlEncode(dt.ToString("yyyy-MM-dd 00:00:00"));
        }

        public async Task<List<ClipsalUsage>> _get(string queryString)
        {
            try
            {


                using (var client = _httpClientFactory.CreateClient())
                {
                    _logService.WriteLog($"Getting Clipsal data: {_settings.Value.CLIPSAL_DATA_URL}");
                    
                    var uri = new Uri(_settings.Value.CLIPSAL_DATA_URL + $"?{queryString}");
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

        public async Task<ClipsalInstant> GetInstant()
        {
            try
            {


                using (var client = _httpClientFactory.CreateClient())
                {
                    _logService.WriteLog($"Getting Clipsal data: {_settings.Value.CLIPSAL_INST_URL}");

                    var uri = new Uri(_settings.Value.CLIPSAL_INST_URL);
                    var result = await client.GetAsync(uri);
                    if (!result.IsSuccessStatusCode)
                    {
                        _logService.WriteError($"Error in get clipsa; data: {result.ReasonPhrase}");
                        return null;
                    }

                    var s = await result.Content.ReadAsStringAsync();

                    var model = JsonConvert.DeserializeObject<AmberServerResponse>(s);

                    var parsed = JsonConvert.DeserializeObject<ClipsalInstant>(model.Usage);



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
