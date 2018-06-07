using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.NetTests;

namespace Utils.Net.Interactivity.Behaviors.Tests
{
    [TestClass]
    public class ListViewExtensionBehaviorTests
    {
        private ListView testListView;
        private ListViewExtensionBehavior testBehavior;
        private IList testSelectedItems;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            UITester.Dispatcher.Invoke(() => UITester.Get<ComboBox>().SelectedItem = "ExplorerPage");
            System.Threading.Thread.Sleep(100);

            testListView = UITester.Get<ListView>();
            testBehavior = UITester.Dispatcher.Invoke(() => Interaction.GetBehaviors(testListView).OfType<ListViewExtensionBehavior>().Single());
            testSelectedItems = UITester.Dispatcher.Invoke(() => testBehavior.SelectedItems as IList);
        }

        [TestMethod]
        public void MultiSelectedItemsTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                testListView.SelectedItems.Add(testListView.Items[2]);
                testListView.SelectedItems.Add(testListView.Items[5]);
                CollectionAssert.AreEqual(testListView.SelectedItems, testSelectedItems);

                testListView.SelectedItems.RemoveAt(1);
                CollectionAssert.AreEqual(testListView.SelectedItems, testSelectedItems);

                testSelectedItems.Add(testListView.Items[6]);
                CollectionAssert.AreEqual(testListView.SelectedItems, testSelectedItems);

                testSelectedItems.Remove(testListView.Items[6]);
                CollectionAssert.AreEqual(testListView.SelectedItems, testSelectedItems);

                // for code coverage
                testBehavior.SelectedItems = null;
                testListView.SelectedItems.Add(testListView.Items[3]);

                testBehavior.SelectedItems = new ObservableCollection<string>();
                testListView.SelectedItems.RemoveAt(0);
                testListView.SelectedItems.Add(testListView.Items[1]);

                testBehavior.SelectedItems = testSelectedItems as System.Collections.Specialized.INotifyCollectionChanged;
                testSelectedItems.Clear();

                testBehavior.Detach();
                testBehavior.Attach(testListView);
            });
        }

        [TestMethod]
        public void DeselectTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                testListView.SelectedItems.Add(testListView.Items[1]);
                testListView.RaiseEvent(
                    new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                    {
                        RoutedEvent = Mouse.PreviewMouseDownEvent
                    });
                Assert.AreEqual(testListView.SelectedItems.Count, 0);

                // for code coverage
                var isDeselectEnable = testBehavior.IsDeselectEnable;
                testBehavior.IsDeselectEnable = false;
                testListView.RaiseEvent(
                    new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                    {
                        RoutedEvent = Mouse.PreviewMouseDownEvent
                    });
                testBehavior.IsDeselectEnable = isDeselectEnable;
            });
        }
    }
}
