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

            await service.SendNotification("Testing 123 from test thingo", "This is the title");
        }

        [TestMethod]
        public async Task TestThresholds()
        {
            var service = Resolve<IThresholdingService>();

            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("CurrentPriceIn", 40d);
            dict.Add("TestMode", true);

            await service.RunChecks(dict);

            await Task.Delay(2000);

            dict.Clear();
            dict.Add("CurrentPriceIn", 80d);
            dict.Add("TestMode", true);

            await service.RunChecks(dict);

            await Task.Delay(2000);

            dict.Clear();
            dict.Add("CurrentPriceIn", 200d);
            dict.Add("TestMode", true);

            await service.RunChecks(dict);

            await Task.Delay(2000);

            dict.Clear();
            dict.Add("CurrentPriceIn", 18d);
            dict.Add("TestMode", true);

            await service.RunChecks(dict);
        }
    }
}
