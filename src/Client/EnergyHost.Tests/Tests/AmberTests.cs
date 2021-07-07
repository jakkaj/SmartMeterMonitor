using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class AmberTests : TestBase
    {
        [TestMethod]
        public async Task TestComposeV2Local()
        {
            var data = File.ReadAllText("c:\\temp\\amberdatatest.json");

            var amberData = JsonConvert.DeserializeObject<AmberGraphDataParsed>(data);

            var aService = Resolve<IAmberServiceV2>();

            var days = aService.Compose(amberData);
            foreach (var d in days.Days)
            {
                Debug.WriteLine(d.ActualPriceInCents / 100);
            }
        }

        [TestMethod]
        public async Task GetDataV2()
        {
            var aService = Resolve<IAmberServiceV2>();

            var amberData = await aService.Get();

            var json = JsonConvert.SerializeObject(amberData);

            File.WriteAllText("c:\\temp\\amberdatatest.json", json);

            Assert.IsNotNull(amberData);


        }


        [TestMethod]
        public async void GetConfig()
        {
            var config = Resolve<IOptions<EnergyHostSettings>>();

            Assert.IsNotNull(config.Value.AMBER_API_URL);
        }

        [TestMethod]
        public async Task GetData()
        {
            var aService = Resolve<IAmberService>();

            var amberData = await aService.Get("2047");

            foreach (var i in amberData.data.variablePricesAndRenewables)
            {
                var s = "";
                for (var icount = 0.0; icount < i.InPriceNormal; icount+= 0.01)
                {
                    s += "-";
                }
                Debug.WriteLine($"{i.period.ToLongDateString()} {i.period.ToLongTimeString()}:[{i.periodType}] {s}");
            }

            Assert.IsNotNull(amberData);
        }

        [TestMethod]
        public async Task GetUsage()
        {
            var aService = Resolve<IAmberService>();

            var result = await aService.GetUsage();

            Assert.IsNotNull(result);
        }
    }
}
