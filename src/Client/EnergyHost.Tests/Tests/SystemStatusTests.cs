using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels.Status;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class SystemStatusTests : TestBase
    {
        
        [TestMethod]
        public async Task TestSystemStatusSer()
        {
            var m = Resolve<IMQTTService>();
            await m.Setup();
            await Task.Delay(2000);
            var service = Resolve<ISystemStatusService>();

            var t = new TimeStatus();

            await service.SendStatus(t);

            await Task.Delay(1500);

            t = new TimeStatus();

            await service.SendStatus(t);

            await Task.Delay(1500);

            t = new TimeStatus();

            await service.SendStatus(t);

            await Task.Delay(1500);

            t = new TimeStatus();

            await service.SendStatus(t);

            await Task.Delay(1500);

            while (service.GetStatus<TimeStatus>().latest == null)
            {
                await Task.Delay(500);
;            }

            var status = service.GetStatus<TimeStatus>();

            Assert.IsNotNull(status);

            Debug.WriteLine(status.latest.CurrentDateTime);

            DateTime current = DateTime.Now.Add(TimeSpan.FromSeconds(15));

            //test history is reversed properly. 
            Debug.WriteLine($"Id: {status.latest.StatusId} DateTime: {status.latest.CurrentDateTime}.{status.latest.CurrentDateTime.Millisecond}");

            foreach (var i in status.history)
            {
                Debug.WriteLine($"Id: {i.StatusId} DateTime: {i.CurrentDateTime}.{i.CurrentDateTime.Millisecond}");
                Assert.IsTrue(i.CurrentDateTime < current);

                current = i.CurrentDateTime;
            }


        }
    }
}
