using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyHost.Tests.Tests
{
    [TestClass]
    public class NotificationTests :TestBase
    {
        [TestMethod]
        public async Task TestNotify()
        {
            var service = Resolve<INotificationService>();

            await service.SendNotification("Testing 123 from test thingo");
        }
    }
}
