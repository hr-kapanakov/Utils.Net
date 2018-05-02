using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Common.Tests
{
    [TestClass]
    public class EventArgsTests
    {
        [TestMethod]
        public void EventArgsTest()
        {
            var value = "test";
            var args = new EventArgs<string>(value);
            Assert.AreEqual(value, args.Value);
        }
    }
}
