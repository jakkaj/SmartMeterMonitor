using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Helpers;

namespace Smart.Tests
{
    [TestClass]
    public class KWTests
    {
        [TestMethod]
        public void TestKWHCalcs()
        {
            var result = KWHelper.CalcKWH(5, 15);
            Debug.WriteLine(result);
            Assert.IsTrue(result != 0);
        }

        [TestMethod]
        public async Task TestPostToPowerBi()
        {
            //await PowerBIHelper.Push(1.20);
        }
    }
}
