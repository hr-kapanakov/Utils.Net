using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Interactivity.Behaviors.Tests
{
    [TestClass]
    public class ListViewExtensionBehaviorTest
    {
        private ListView testListView;
        private ObservableCollection<int> testSelectedItems;
        private ListViewExtensionBehavior testBehavior;

        [TestInitialize]
        public void SetUp()
        {
            testListView = new ListView
            {
                ItemsSource = Enumerable.Range(0, 10)
            };

            testSelectedItems = new ObservableCollection<int>();
            testBehavior = new ListViewExtensionBehavior
            {
                SelectedItems = testSelectedItems
            };
            testBehavior.Attach(testListView);
        }

        [TestCleanup]
        public void CleanUp()
        {
            testBehavior.Detach();
        }

        [TestMethod]
        public void MultiSelectedItemsTest()
        {
            testListView.SelectedItems.Add(2);
            testListView.SelectedItems.Add(5);
            CollectionAssert.AreEqual(testListView.SelectedItems, testSelectedItems);

            testListView.SelectedItems.RemoveAt(1);
            CollectionAssert.AreEqual(testListView.SelectedItems, testSelectedItems);

            ((ObservableCollection<int>)testBehavior.SelectedItems).Add(6);
            CollectionAssert.AreEqual(testListView.SelectedItems, testSelectedItems);

            ((ObservableCollection<int>)testBehavior.SelectedItems).Remove(6);
            CollectionAssert.AreEqual(testListView.SelectedItems, testSelectedItems);
            
            // for code coverage
            testBehavior.SelectedItems = null;
            testListView.SelectedItems.Add(2);
        }
    }
}
