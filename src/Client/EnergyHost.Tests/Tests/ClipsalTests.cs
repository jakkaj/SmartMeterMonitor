using EnergyHost.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class ClipsalTests : TestBase
    {
        [TestMethod]
        public async Task TestGetClipsal()
        {
            var s = Resolve<IClipsalService>();

            var data = await s.Get();

            Assert.IsNotNull(data);

            foreach(var d in data)
            {
                Debug.WriteLine(d.date);
            }
        }
    }
}
