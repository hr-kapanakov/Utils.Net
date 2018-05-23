using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.ViewModels;

namespace Utils.Net.Extensions.Tests
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        private IEnumerable<int> testEnumerable = Enumerable.Range(0, 10);


        [TestMethod]
        public void FlattenTest()
        {
            var root = new TreeItemViewModel<string>("0");
            root.Children.Add(new TreeItemViewModel<string>("1", root));
            root.Children[0].Children.Add(new TreeItemViewModel<string>("11", root.Children[0]));
            root.Children.Add(new TreeItemViewModel<string>("2", root));

            Assert.ThrowsException<ArgumentNullException>(() => root.Children.Flatten(null));

            var flat = root.Children.Flatten(c => c.Children).OfType<TreeItemViewModel<string>>();
            Assert.AreEqual(flat.Count(), 3);
            Assert.IsTrue(flat.All(f => f.Content == "0" || f.Content == "1" || f.Content == "11" || f.Content == "2"));
        }

        [TestMethod]
        public void RangeTest()
        {
            var subSet = testEnumerable.Range(3, 5).ToList();
            CollectionAssert.AreEqual(subSet, Enumerable.Range(3, 5).ToList());
        }

        [TestMethod]
        public void ForEachTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => testEnumerable.ForEach(null));

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
