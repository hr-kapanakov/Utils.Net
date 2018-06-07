using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.NetTests;

namespace Utils.Net.Interactivity.Behaviors.Tests
{
    [TestClass]
    public class TreeViewExtensionBehaviorTests
    {
        private TreeView testTreeView;
        private TreeViewExtensionBehavior testBehavior;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            UITester.Dispatcher.Invoke(() => UITester.Get<ComboBox>().SelectedItem = "ExplorerPage");
            System.Threading.Thread.Sleep(100);

            testTreeView = UITester.Get<TreeView>();
            testBehavior = UITester.Dispatcher.Invoke(() => Interaction.GetBehaviors(testTreeView).OfType<TreeViewExtensionBehavior>().Single());
        }

        [TestMethod]
        public void SelectedItemTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                var treeViewItem = testTreeView.ItemContainerGenerator.ContainerFromItem(testTreeView.Items[0]) as TreeViewItem;
                treeViewItem.IsSelected = true;
                Assert.AreEqual(testBehavior.SelectedItem, testTreeView.Items[0]);

                testBehavior.SelectedItem = null;
                Assert.IsFalse(treeViewItem.IsSelected);

                // for code coverage
                testBehavior.Detach();
                testBehavior.Attach(testTreeView);
            });
        }

        [TestMethod]
        public void DeselectTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                testBehavior.SelectedItem = testTreeView.Items[1];
                testTreeView.RaiseEvent(
                    new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                    {
                        RoutedEvent = Mouse.PreviewMouseDownEvent
                    });
                Assert.IsNull(testTreeView.SelectedItem);

                // for code coverage
                var isDeselectEnable = testBehavior.IsDeselectEnable;
                testBehavior.IsDeselectEnable = false;
                testTreeView.RaiseEvent(
                    new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                    {
                        RoutedEvent = Mouse.PreviewMouseDownEvent
                    });
                testBehavior.IsDeselectEnable = isDeselectEnable;
            });
        }
    }
}
