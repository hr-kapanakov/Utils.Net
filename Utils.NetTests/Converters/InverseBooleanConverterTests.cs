using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Net.Converters.Tests
{
    [TestClass]
    public class InverseBooleanConverterTests
    {
        private const bool testValue = true;

        private readonly InverseBooleanConverter testConverter = new InverseBooleanConverter();


        [TestMethod]
        public void ConvertTest()
        {
            var inverse = testConverter.Convert(testValue, null, null, null);
            Assert.IsInstanceOfType(inverse, typeof(bool));
            Assert.AreEqual(inverse, !testValue);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            var inverse = testConverter.ConvertBack(!testValue, null, null, null);
            Assert.IsInstanceOfType(inverse, typeof(bool));
            Assert.AreEqual(inverse, testValue);
        }
    }
}