using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Converters.Tests
{
    [TestClass]
    public class CombiningConverterTests
    {
        private const string TestUriPath = "/Images/icon.ico";

        private readonly CombiningConverter testConverter = new CombiningConverter();


        [TestInitialize]
        public void SetUp()
        {
            testConverter.Converter1 = new StringToUriConverter();
            testConverter.Converter2 = new TypeOfConverter();
        }

        [TestMethod]
        public void ConvertTest()
        {
            var type = testConverter.Convert(TestUriPath, null, null, null);
            Assert.IsInstanceOfType(type, typeof(Type));
            Assert.AreEqual(type, typeof(Uri));
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            // TODO: need 2 backable converters
        }
    }
}
