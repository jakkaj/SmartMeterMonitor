using System;
using System.Diagnostics;
using EnergyHost.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class DataLoggerTests : TestBase
    {

        //[TestMethod]
        //public async Task TestTiming()
        //{
        //    //while (true)
        //    //{
        //    //    Debug.WriteLine($"{DateTime.Now.Minute % 5} - Secs {DateTime.Now.Second}");
        //    //    await Task.Delay(TimeSpan.FromSeconds(1));
        //    //}
        //}

        //[TestMethod]
        public async Task TestLoggerPolling()
        {
            var ser = Resolve<IDataLoggerService>();
            await ser.Start();

            await Task.Delay(-1);
        }
    }
}
