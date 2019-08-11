using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class DataLoggerTests : TestBase
    {
        [TestMethod]
        public async Task TestLoggerPolling()
        {
            var ser = Resolve<IDataLoggerService>();
            await ser.Start();

            await Task.Delay(-1);
        }
    }
}
