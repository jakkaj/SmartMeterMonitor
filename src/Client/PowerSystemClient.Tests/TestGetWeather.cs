

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PowerSystemClient.Tests
{
    [TestClass]
    public class TestGetWeather : TestBase
    {
        [TestMethod]
        public async Task GetNowForCity()

        {
            var apiKey = Config["OPEN_WEATHER_API_KEY"];

            Assert.IsNotNull(apiKey);

            var weatherApi = new OpenWeatherMap.OpenWeatherMap(apiKey);

            var balmain = await weatherApi.GetCurrent("Balmain");

            Assert.IsNotNull(balmain);

            Debug.WriteLine(balmain.Main.Temp);
        }

        [TestMethod]
        public async Task GetForecastForCity()

        {
            var apiKey = Config["OPEN_WEATHER_API_KEY"];

            Assert.IsNotNull(apiKey);

            var weatherApi = new OpenWeatherMap.OpenWeatherMap(apiKey);

            var balmain = await weatherApi.GetForecast("Balmain");

            Assert.IsNotNull(balmain);

            Debug.WriteLine(balmain.List[0].Main.Temp);

            var maxmax = await weatherApi.GetMaximums("Balmain");

            Assert.IsTrue(maxmax.today > 0);//yeah yeah, it doesn't get very cold here:P
            Assert.IsTrue(maxmax.tomorrow > 0);

            var minmin = await weatherApi.GetMinimums("Balmain");

            Assert.IsTrue(minmin.today > 0);//yeah yeah, it doesn't get very cold here:P
            Assert.IsTrue(minmin.tomorrow > 0);
        }
    }
}
