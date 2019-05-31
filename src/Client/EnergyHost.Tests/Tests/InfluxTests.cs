using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class InfluxTests : TestBase
    {
        [TestMethod]
        public async Task TestWrite()
        {
            var s = Resolve<IInfluxService>();

            var data = new Dictionary<string, object>
            {
                {"reading", "1232"}
            };

            await s.Write("test", "jktest", data);
        }
    }
}
