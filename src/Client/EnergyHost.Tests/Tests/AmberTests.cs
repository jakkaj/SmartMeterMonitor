using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class AmberTests : TestBase
    {
        [TestMethod]
        public async Task GetConfig()
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
                for (var icount = 0; icount < i.InPriceNormal; icount++)
                {
                    s += "-";
                }
                Debug.WriteLine($"{i.period.ToLongDateString()} {i.period.ToLongTimeString()}:[{i.periodType}] {s}");
            }

            Assert.IsNotNull(amberData);
        }
    }
}
