using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Converters.Tests
{
    [TestClass]
    public class TypeOfConverterTests
    {
        private const string testObject = "test";

        private readonly TypeOfConverter testConverter = new TypeOfConverter();


        [TestMethod]
        public void ConvertTest()
        {
            var type = testConverter.Convert(testObject, null, null, null);
            Assert.IsInstanceOfType(type, typeof(Type));
            Assert.AreEqual(type, testObject.GetType());
        }
    }
}