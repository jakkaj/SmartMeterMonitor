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
        public async Task TestWriteLineProtocol()
        {
            var s = Resolve<IInfluxService>();

            var data = new Dictionary<string, object>
            {
                {"reading", "1234"}

            };

            var result = await s.Write("test", "jktest",data, null, DateTime.UtcNow.AddMinutes(-3600));
            Assert.IsTrue(result);
            //Console.WriteLine("testing 123");
        }

        [TestMethod]
        public async Task TestWrite()
        {
            var s = Resolve<IInfluxService>();

            var data = new Dictionary<string, object>
            {
                {"time", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
                {"reading", "1234"}
                
            };

            await s.Write("test", "jktest", data);
            Console.WriteLine("testing 123");
        }
    }
}
