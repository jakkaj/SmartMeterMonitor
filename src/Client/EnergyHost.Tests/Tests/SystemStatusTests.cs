﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels.Status;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class SystemStatusTests : TestBase
    {
        
        [TestMethod]
        public async Task TestSystemStatusSer()
        {
            var m = Resolve<IMQTTService>();
            await m.Setup();
            var service = Resolve<ISystemStatusService>();

            var t = new TimeStatus();

            await service.SendStatus(t);

            await Task.Delay(10000);
        }
    }
}
