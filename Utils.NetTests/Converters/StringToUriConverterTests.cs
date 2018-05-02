using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Converters.Tests
{
    [TestClass]
    public class StringToUriConverterTests
    {
        private const string TestUriPath = "/Images/icon.ico";

        private readonly StringToUriConverter testConverter = new StringToUriConverter();


        [TestMethod]
        public void ConvertTest()
        {
            var uri = testConverter.Convert(TestUriPath, null, null, null);
            Assert.IsInstanceOfType(uri, typeof(Uri));
            Assert.AreEqual(uri.ToString(), TestUriPath);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            var path = testConverter.ConvertBack(null, null, null, null);
            Assert.IsInstanceOfType(path, typeof(string));
            Assert.AreEqual((string)path, string.Empty);

            var uri = new Uri(TestUriPath, UriKind.RelativeOrAbsolute);
            path = testConverter.ConvertBack(uri, null, null, null);
            Assert.IsInstanceOfType(path, typeof(string));
            Assert.AreEqual((string)path, TestUriPath);
        }
    }
}
