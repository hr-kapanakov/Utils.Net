using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Converters.Tests
{
    [TestClass]
    public class MultiBooleanConverterTests
    {
        private readonly object[] testArray1 = { true, true, true };
        private readonly object[] testArray2 = { true, true, false };

        private readonly MultiBooleanConverter testConverter = new MultiBooleanConverter();


        [TestMethod]
        public void ConvertTest()
        {
            var result = testConverter.Convert(testArray1, null, null, null);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.AreEqual(result, true);

            result = testConverter.Convert(testArray2, null, null, null);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            Assert.ThrowsException<InvalidOperationException>(() => testConverter.ConvertBack(testArray1, null, null, null));
        }
    }
}
