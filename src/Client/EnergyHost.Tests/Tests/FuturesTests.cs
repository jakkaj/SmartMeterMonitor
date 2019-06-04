using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class FuturesTests : TestBase
    {
        [TestMethod]
        public async Task TestFutures()
        {
            var fService = Resolve<IEnergyFuturesService>();

            var f = await fService.Get();

            foreach (var energryFuture in f.Futures)
            {
                var s = "";
                for (var icount = 0D; icount < energryFuture.GridValue; icount += 0.02)
                {
                    s += "-";
                }

                for (var icount = 0D; icount < energryFuture.SolarValue; icount += 0.02)
                {
                    s += "=";
                }
                Debug.WriteLine($"{energryFuture.Period} : {energryFuture.PriceIn.ToString("F")} : {s}");
                
            }
        }
    }
}
