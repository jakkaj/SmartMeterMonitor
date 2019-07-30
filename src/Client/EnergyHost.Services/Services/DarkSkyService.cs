using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DarkSky.Models;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;

namespace EnergyHost.Services.Services
{
    public class DarkSkyService : IDarkSkyService
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;

        public DarkSkyService(ILogService logService, IOptions<EnergyHostSettings> settings)
        {
            _logService = logService;
            _settings = settings;
        }

        public async Task<(double temp, double humid, double pressure,
            double wind, double minToday, double maxToday,
            double minTomorrow, double maxTomorrow, double cloudiness)> GetDetail()
        {
            var forecast = await Get();
            var humid = forecast.Currently.Humidity ?? 0;

            var temp = forecast.Currently.Temperature ?? 0;

            var pressure = forecast.Currently.Pressure ?? 0;

            var wind = forecast.Currently.WindSpeed ?? 0;

            var minToday = forecast.Daily.Data[0].TemperatureLow ?? 0;
            var maxToday = forecast.Daily.Data[0].TemperatureHigh ?? 0;

            var cloudiness = forecast.Daily.Data[0].CloudCover ?? 0;

            var minTomorrow = forecast.Daily.Data[1].TemperatureLow ?? 0;
            var maxTomorrow = forecast.Daily.Data[1].TemperatureHigh ?? 0;


            return (temp, humid, pressure, wind, minToday, maxToday, minTomorrow, maxTomorrow, cloudiness);
        }

        public async Task<Forecast> Get()
        {
            var weather = new DarkSky.Services.DarkSkyService(_settings.Value.DARK_SKY_API_KEY);

            var forecast = await weather.GetForecast(_settings.Value.Latitude, _settings.Value.Longitude, new DarkSky.Services.DarkSkyService.OptionalParameters
            {
                MeasurementUnits = "si",
            });
            
            return forecast.Response;
        }
    }
}
