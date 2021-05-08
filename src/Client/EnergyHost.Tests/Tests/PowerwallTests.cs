﻿using System;
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