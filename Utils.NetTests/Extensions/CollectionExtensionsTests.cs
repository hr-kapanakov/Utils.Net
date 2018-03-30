using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Extensions.Tests
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        private Collection<int> testCollection = new ObservableCollection<int>();

        [TestMethod]
        public void AddRangeTest()
        {
            var list = Enumerable.Range(0, 10).ToList();
            testCollection.AddRange(list);
            Assert.IsTrue(list.All(i => testCollection.Contains(i)));
        }

        [TestMethod]
        public void UpdateCollectionTest()
        {
            var list = Enumerable.Range(0, 10).ToList();
            testCollection.UpdateCollection(list);
            CollectionAssert.AreEqual(testCollection, list);

            list.RemoveAt(3);
            testCollection.UpdateCollection(list);
            CollectionAssert.AreEqual(testCollection, list);

            list.Add(100);
            testCollection.UpdateCollection(list);
            CollectionAssert.AreEqual(testCollection, list);
        }
    }
}