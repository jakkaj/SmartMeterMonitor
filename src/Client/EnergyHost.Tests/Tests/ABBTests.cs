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
    public class ABBTests:TestBase
    {
        [TestMethod]
        public async Task TestABBModbusData()
        {
            var service = Resolve<IABBService>();
            var model = await service.GetModbus();
            Assert.IsNotNull(model);
        }
        [TestMethod]
        public async Task TestABBGetData()
        {
            var service = Resolve<IABBService>();

            var result = await service.Get();

            Assert.IsNotNull(result);

            var pout = result.feeds.Feed.datastreams.m101_1_W.data[0].value;
            Debug.WriteLine(pout);
        }
    }
}
