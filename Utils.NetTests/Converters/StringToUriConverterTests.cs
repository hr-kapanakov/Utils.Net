using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Converters.Tests
{
    [TestClass]
    public class StringToUriConverterTests
    {
        private const string testUriPath = "/Images/icon.ico";

        private readonly StringToUriConverter testConverter = new StringToUriConverter();


        [TestMethod]
        public void ConvertTest()
        {
            var uri = testConverter.Convert(testUriPath, null, null, null);
            Assert.IsInstanceOfType(uri, typeof(Uri));
            Assert.AreEqual(uri.ToString(), testUriPath);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            var uri = new Uri(testUriPath, UriKind.RelativeOrAbsolute);
            var path = testConverter.ConvertBack(uri, null, null, null);
            Assert.IsInstanceOfType(path, typeof(string));
            Assert.AreEqual(((string)path), testUriPath);
        }
    }
}