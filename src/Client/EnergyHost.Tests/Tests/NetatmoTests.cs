using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Services.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class NetatmoTests : TestBase
    {
        [TestMethod]
        public async Task TestNetatmo()
        {
            var netatmoService = Resolve<INetatmoService>();

            var result = await netatmoService.Get();

            Assert.IsNotNull(result);
        }
    }
}
