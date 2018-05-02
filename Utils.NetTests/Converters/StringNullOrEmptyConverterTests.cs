using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Converters.Tests
{
    [TestClass]
    public class StringNullOrEmptyConverterTests
    {
        private const string TestNonEmptyString = "test";
        private const string TestEmptyString = "";

        private readonly StringNullOrEmptyConverter testConverter = new StringNullOrEmptyConverter();


        [TestMethod]
        public void ConvertTest()
        {
            var isNullOrEmpty = testConverter.Convert(TestNonEmptyString, null, null, null);
            Assert.IsInstanceOfType(isNullOrEmpty, typeof(bool));
            Assert.AreEqual(isNullOrEmpty, false);

            isNullOrEmpty = testConverter.Convert(TestEmptyString, null, null, null);
            Assert.IsInstanceOfType(isNullOrEmpty, typeof(bool));
            Assert.AreEqual(isNullOrEmpty, true);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            Assert.ThrowsException<InvalidOperationException>(() => testConverter.ConvertBack(TestNonEmptyString, null, null, null));
        }
    }
}
