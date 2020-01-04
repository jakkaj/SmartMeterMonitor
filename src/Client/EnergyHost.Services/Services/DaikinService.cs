using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EnergyHost.Contract;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.EnergyModels.Status;
using EnergyHost.Model.Settings;
using EnergyHost.Services.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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

        public async Task<bool> PowerOff()
        {
            var settings = await GetControlInfo();

            var settings2 = JsonConvert.DeserializeObject<DaikinSettings>(JsonConvert.SerializeObject(settings));

            settings.pow = "0";

            if (isSameSettings(settings, settings2) || settings.mode == "0") //don't power off if already on fan 
            {
                return false;
            }

            await SetControlInfo(settings);
            _logService.WriteLog("***** Powered off Daikin *****");
            return true;
        }

        private bool isSameSettings(DaikinSettings a, DaikinSettings b)
        {
            var qsa = a.GetQueryString();
            var qsb = b.GetQueryString();

            return qsa == qsb;
        }

        public async Task SetControlInfo(DaikinSettings settings)
        {
            var qs = settings.GetQueryString();
            await _setDaikin(qs);
        }
        

        public async Task<DaikinSettings> GetControlInfo()
        {
            var data = await _getDaikin("get_control_info");

            if (data == null)
            {
                return null;
            }

            var qs = HttpUtility.ParseQueryString(data.Replace(",", "&"));
            string json = JsonConvert.SerializeObject(qs.Cast<string>().ToDictionary(k => k, v => qs[v]));
            var respObj = JsonConvert.DeserializeObject<DaikinSettings>(json);

            return respObj;
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

        async Task<string> _setDaikin(string qs)
        {
            var url = $"{_settings.Value.DAIKIN_URL}/set_control_info?lpw={_settings.Value.DAIKIN_AUTH}&{qs}";

            using (var client = new HttpClient())
            {
                var retryCount = 0;
                while (retryCount < 3)
                {
                    try
                    {
                        var result = await client.GetAsync(url);

                        if (!result.IsSuccessStatusCode)
                        {
                            return null;
                        }
                        return await result.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        _logService.WriteError(ex.ToString());
                    }
                    retryCount++;

                    _logService.WriteLog($"Daikin service set retry: {retryCount}");
                }

                return null;
            }
        }



        async Task<string> _getDaikin(string service)
        {
            var url = $"{_settings.Value.DAIKIN_URL}/{service}?lpw={_settings.Value.DAIKIN_AUTH}";

            using (var client = new HttpClient())
            {
                var retryCount = 0;
                while (retryCount < 3)
                {
                    try
                    {
                        var result = await client.GetAsync(url);

                        if (!result.IsSuccessStatusCode)
                        {
                            return null;
                        }
                        return await result.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        _logService.WriteError(ex.ToString());
                    }
                    retryCount ++;

                    _logService.WriteLog($"Daikin service retry: {retryCount}");
                }

                return null;
            }
        }
    }
}
