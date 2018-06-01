using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Common.Tests
{
    [TestClass]
    public class BindingProxyTests
    {
        [TestMethod]
        public void BindingProxyTest()
        {
            var testBindingProxy = new BindingProxy
            {
                Data = "test"
            };

            // for code coverage
            testBindingProxy.Clone();

            Assert.AreEqual(testBindingProxy.Data, "test");
            Assert.IsInstanceOfType(testBindingProxy.As<string>(), typeof(string));
        }
    }
}
