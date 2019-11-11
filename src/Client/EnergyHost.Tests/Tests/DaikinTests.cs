using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class DaikinTests : TestBase
    {
        [TestMethod]
        public async Task TestDaikinSensors()
        {
            var service = Resolve<IDaikinService>();

            var data = await service.GetSensors();
            Assert.IsNotNull(data);
            var temp = data["htemp"];
        }

        [TestMethod]
        public async Task TestDaikinGetControl()
        {
            var service = Resolve<IDaikinService>();

            var data = await service.GetControlInfo();
            Assert.IsNotNull(data);
            var temp = data.stemp;
            Assert.IsNotNull(temp);
        }

        [TestMethod]
        public async Task TestDaikinSetControl()
        {
            var service = Resolve<IDaikinService>();

            var data = await service.GetControlInfo();
            Assert.IsNotNull(data);
            var temp = data.stemp;
            Assert.IsNotNull(temp);

            data.pow = "1";
            data.stemp = "27";

            await service.SetControlInfo(data);
        }
    }
}
