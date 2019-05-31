using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class TestMQTT : TestBase
    {
        [TestMethod]
        public async Task TestConnection()
        {
            var s = Resolve<IMQTTService>();

            await s.Setup();

            await Task.Delay(TimeSpan.FromSeconds(30));
        }
    }
}
