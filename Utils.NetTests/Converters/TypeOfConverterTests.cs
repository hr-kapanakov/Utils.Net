using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Converters.Tests
{
    [TestClass]
    public class TypeOfConverterTests
    {
        private const string TestObject = "test";

        private readonly TypeOfConverter testConverter = new TypeOfConverter();


        [TestMethod]
        public void ConvertTest()
        {
            Assert.IsNull(testConverter.Convert(null, null, null, null));

            var type = testConverter.Convert(TestObject, null, null, null);
            Assert.IsInstanceOfType(type, typeof(Type));
            Assert.AreEqual(type, TestObject.GetType());
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            Assert.ThrowsException<InvalidOperationException>(() => testConverter.ConvertBack(TestObject, null, null, null));
        }
    }
}
