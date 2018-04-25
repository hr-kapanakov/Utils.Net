using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Converters.Tests
{
    [TestClass]
    public class StringNullOrEmptyConverterTests
    {
        private const string testNonEmptyString = "test";
        private const string testEmptyString = "";

        private readonly StringNullOrEmptyConverter testConverter = new StringNullOrEmptyConverter();


        [TestMethod]
        public void ConvertTest()
        {
            var isNullOrEmpty = testConverter.Convert(testNonEmptyString, null, null, null);
            Assert.IsInstanceOfType(isNullOrEmpty, typeof(bool));
            Assert.AreEqual(isNullOrEmpty, false);

            isNullOrEmpty = testConverter.Convert(testEmptyString, null, null, null);
            Assert.IsInstanceOfType(isNullOrEmpty, typeof(bool));
            Assert.AreEqual(isNullOrEmpty, true);
        }
    }
}