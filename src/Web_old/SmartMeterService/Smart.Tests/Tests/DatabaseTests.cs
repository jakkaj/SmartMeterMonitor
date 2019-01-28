using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Helpers.Contracts;
using Smart.Helpers.DB;

namespace Smart.Tests.Tests
{
    [TestClass]
    public class DatabaseTests : TestBase
    {
        [TestMethod]
        public async Task TestConnection()
        {
            var c = Resolve<PowerContext>();


            var items = await c.PowerReadings.FirstOrDefaultAsync();

            Assert.IsNotNull(items);
        }

        [TestMethod]
        public async Task TestAverageSoFarToday()
        {
            var c = Resolve<IDatabaseService>();
            var average = await c.AverageSoFarToday();
            Assert.IsTrue(average > -1);

        }

        [TestMethod]
        public async Task TestAverageSoFarYesterday()
        {
            var c = Resolve<IDatabaseService>();
            var average = await c.AverageSoFarYesterday();
            Assert.IsTrue(average > -1);

        }

        [TestMethod]
        public async Task TestAverageLast24Hours()
        {
            var c = Resolve<IDatabaseService>();
            var average = await c.AverageLast24Hours();
            Assert.IsTrue(average > -1);

        }

        [TestMethod]
        public async Task TestAverageYesterday()
        {
            var c = Resolve<IDatabaseService>();
            var average = await c.AverageYesterday();
            Assert.IsTrue(average > -1);

        }
    }
}
