using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;
using NetatmoCore;
using netatmocore.Models;

namespace EnergyHost.Services.Services
{
    public class NetatmoService : Contract.INetatmoService
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public NetatmoService(ILogService logService, IOptions<EnergyHostSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _logService = logService;
            _settings = settings;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<NetatmoData> Get()
        {
            var auth = new NetatmoAuth();
            var token = auth.Login(
                _settings.Value.NETATMO_CLIENT_ID, 
                _settings.Value.NETATMO_CLIENT_SECRET, 
                _settings.Value.NETATMO_USER_NAME, 
                _settings.Value.NETATMO_PASSWORD, 
                new[] { NetatmoAuth.READ_STATION });
            var netatmo = new NetAtmoClient(token.access_token);

            var result = await netatmo.Getthermostatsdata(_settings.Value.NETATMO_DEVICE_ID);

            var indoorData = result.Body.Devices[0].DashboardData;
            var outdoorData = result.Body.Devices[0].Modules[0].DashboardData;
            var windData = result.Body.Devices[0].Modules[1].DashboardData;
            var rainData = result.Body.Devices[0].Modules[2].DashboardData;

            var netatmoData = new NetatmoData
            {
                IndoorTemp = indoorData.Temperature,
                OutdoorTemp = outdoorData.Temperature,
                AbsPressure = indoorData.AbsolutePressure,
                Pressure = indoorData.Pressure,
                CO2 = indoorData.CO2,
                IndoorHumidity = indoorData.Humidity,
                OutdoorHumidity = outdoorData.Humidity,
                Noise = indoorData.Noise,
                Rain = rainData.Rain,
                WindAngle = windData.WindAngle,
                WindStrength = windData.WindStrength
            };

            return netatmoData;
        }
    }
}
