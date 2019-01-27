

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PowerSystemClient.Tests
{
    [TestClass]
    public class TestGetWeather : TestBase
    {
        [TestMethod]
        public async Task GetForCity()

        {
            var apiKey = Config["OPEN_WEATHER_API_KEY"];

            Assert.IsNotNull(apiKey);

            var weatherApi = new OpenWeatherMap.OpenWeatherMap(apiKey);

            var balmain = await weatherApi.GetCurrent("Balmain");

            Assert.IsNotNull(balmain);

            Debug.WriteLine(balmain.Main.Temp);
        }
    }
}
