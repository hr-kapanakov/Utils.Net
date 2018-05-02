using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Converters.Tests
{
    [TestClass]
    public class InverseBooleanConverterTests
    {
        private const bool TestValue = true;

        private readonly InverseBooleanConverter testConverter = new InverseBooleanConverter();


        [TestMethod]
        public void ConvertTest()
        {
            var inverse = testConverter.Convert(TestValue, null, null, null);
            Assert.IsInstanceOfType(inverse, typeof(bool));
            Assert.AreEqual(inverse, !TestValue);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            var inverse = testConverter.ConvertBack(!TestValue, null, null, null);
            Assert.IsInstanceOfType(inverse, typeof(bool));
            Assert.AreEqual(inverse, TestValue);
        }
    }
}
