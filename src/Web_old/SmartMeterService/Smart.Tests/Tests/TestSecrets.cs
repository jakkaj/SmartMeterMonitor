using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Smart.Tests.Tests
{
    [TestClass]
    public class TestSecrets : TestBase
    {
        [TestMethod]
        public void TestSecretLoads()
        {
            var secret = SecretOptions.Value.DBConnectionString;
            Assert.IsNotNull(secret);
        }
    }
}
