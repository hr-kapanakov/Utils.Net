using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Extensions.Tests
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        private IEnumerable<int> testEnumerable = Enumerable.Range(0, 10);


        [TestMethod]
        public void ForEachTest()
        {
            int sum = 0;
            testEnumerable.ForEach(i => sum += i);
            Assert.AreEqual(sum, testEnumerable.Sum());
        }

        [TestMethod]
        public void ToObservableCollectionTest()
        {
            var collection = testEnumerable.ToObservableCollection();
            Assert.IsInstanceOfType(collection, typeof(ObservableCollection<int>));
            CollectionAssert.AreEqual(collection, testEnumerable.ToList());
        }
    }
}