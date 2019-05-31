using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class DarkSkyTests : TestBase
    {
        [TestMethod]
        public async Task TestDarkSky()
        {
            var service = Resolve<IDarkSkyService>();
            var forecast  = await service.Get();

            foreach (var i in forecast.Hourly.Data)
            {
                var s = "";
                for (var icount = 0D; icount < i.CloudCover; icount+=0.01)
                {
                    s += "-";
                }
                Debug.WriteLine($"{i.DateTime.ToString()} : {s}");
            }

        }
    }
}
