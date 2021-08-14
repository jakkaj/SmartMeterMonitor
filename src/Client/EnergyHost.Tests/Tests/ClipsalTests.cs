using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class ClipsalTests : TestBase
    {

        [TestMethod]
        public async Task GetInstant()
        {
            var s = Resolve<IClipsalService>();

            var data = await s.GetInstant();

            Assert.IsNotNull(data);
        }

        [TestMethod]
        public async Task PopulateClipsal()
        {
            var s = Resolve<IClipsalService>();
            var imf = Resolve<IInfluxService>();
            for(var i = 1; i < 60; i++)
            {
                
                var d = await s.Get(i);
                var c = s.Compose(d);
                Debug.WriteLine(c[0].date);
                await _writeClipsal(c, imf);
            }
        }

        public async Task _writeClipsal(List<ClipsalInflux> clipsal, IInfluxService influxService)
        {
            foreach (var d in clipsal)
            {
                await influxService.WriteObject("house", $"deviceUsage", d, null, d.date);
            }
        }
        [TestMethod]
        public async Task TestGetClipsal()
        {
            var s = Resolve<IClipsalService>();

            var data = await s.Get(1);

            Assert.IsNotNull(data);

            var composed = s.Compose(data);

            foreach(var i in composed)
            {
                Debug.WriteLine(i.date);
            }
        }
    }
}
