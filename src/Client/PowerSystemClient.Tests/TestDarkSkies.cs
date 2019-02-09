using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DarkSky.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PowerSystemClient.Tests
{
    [TestClass]
    public class TestDarkSkies : TestBase
    {
        readonly string _apiEnvVar = "DARK_SKY_API_KEY";
        readonly double _latitude = -33.86;
        readonly double _longitude = 151.18;
        [TestMethod]
        public async Task TestGetBasicWeatherData()
        {
            var api = Config[_apiEnvVar];

            Assert.IsNotNull(api);

            var weather = new DarkSkyService(api);

            var forecast = await weather.GetForecast(_latitude, _longitude, new DarkSkyService.OptionalParameters
            {
                MeasurementUnits = "si",
            });

            var humid = forecast.Response.Currently.Humidity;
            
            var temp = forecast.Response.Currently.Temperature;

            var wind = forecast.Response.Currently.WindSpeed;

            var minToday = forecast.Response.Daily.Data[0].TemperatureLow;
            var maxToday = forecast.Response.Daily.Data[0].TemperatureHigh;

            var minTomorrow = forecast.Response.Daily.Data[1].TemperatureLow;
            var maxTomorrow = forecast.Response.Daily.Data[1].TemperatureHigh;

            foreach (var day in forecast.Response.Daily.Data)
            {
                var time = day.DateTime.ToLocalTime();
                Debug.WriteLine(time.ToString());
            }


            Debug.WriteLine($"Temp: {temp}, Humid: {humid}");
        }
    }
}
