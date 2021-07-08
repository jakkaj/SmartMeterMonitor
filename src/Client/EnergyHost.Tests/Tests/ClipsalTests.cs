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

            var composed = s.Compose(data);

            foreach(var i in composed)
            {
                Debug.WriteLine(i.date);
            }
        }
    }
}
