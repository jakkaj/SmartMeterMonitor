using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmberElectric.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PowerSystemClient.Tests
{
    [TestClass]
    public class AmberTests : TestBase
    {
        [TestMethod]
        public async Task TestGetAmberData()
        {
            var amber = new AmberService();

            var predict = await amber.Get("2047");

            Assert.IsNotNull(predict);

            var variable = predict.data.variablePricesAndRenewables.OrderBy(_ => _.createdAt)
                .Last(_ => _.periodType == "ACTUAL");

            var inPrice = amber.InPrice(predict, variable);

            var outPrice = amber.OutPrice(predict, variable);
        }

    }
}
