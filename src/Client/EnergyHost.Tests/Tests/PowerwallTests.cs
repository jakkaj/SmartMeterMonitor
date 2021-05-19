using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class PowerwallTests : TestBase
    {
        [TestMethod]
        public async Task ConfigureReserve()
        {
            var pw = Resolve<IPowerwallService>();
            await pw.ConfigureReserve(18, 45);
        }

        [TestMethod]
        public async Task GetReserve()
        {
            var pw = Resolve<IPowerwallService>();

            var result = await pw.GetReservePercent();

            Assert.IsTrue(result >= 0);
        }

        [TestMethod]
        public async Task SetReserve()
        {
            var pw = Resolve<IPowerwallService>();

            await pw.SetReservePercent(50);

            var result = await pw.GetReservePercent();

            Assert.IsTrue(result == 50);

            await Task.Delay(TimeSpan.FromSeconds(10));

            await pw.SetReservePercent(0);

            result = await pw.GetReservePercent();

            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public async Task GetPowerToday()
        {
            var pw = Resolve<IPowerwallService>();

            var result = await pw.GetUsedToday();

            Assert.IsTrue(result > 0);

        }


        [TestMethod]
        public async Task TestGetData()
        {
            var pw = Resolve<IPowerwallService>();

            var data = await pw.GetPowerwall();

            var solar = data.solar.instant_power;
            var grid = data.site.instant_power;
            var battery = data.battery.instant_power;
            var load = data.load.instant_power;

            var charge = data.charge;

            var isCharging = data.battery.instant_power < 0;
            var isDischarging = data.battery.instant_power > 0;






        }
    }
}
