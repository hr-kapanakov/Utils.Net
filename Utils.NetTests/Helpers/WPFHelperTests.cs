using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.ViewModels;
using Utils.NetTests;

namespace Utils.Net.Helpers.Tests
{
    [TestClass]
    public class WPFHelperTests
    {
        private TreeView testTreeView;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));
            
            UITester.Dispatcher.Invoke(() => UITester.Get<ComboBox>().SelectedItem = "ExplorerPage");
            System.Threading.Thread.Sleep(100);

            testTreeView = UITester.Get<TreeView>();
        }

        [TestMethod]
        public void FindVisualAncestorTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                var dummy = testTreeView.FindVisualAncestor<Button>();
                Assert.IsNull(dummy);

                var window = testTreeView.FindVisualAncestor<Window>();
                Assert.AreEqual(window, UITester.MainWindow);

                window = testTreeView.FindVisualAncestor<Window>(w => w.Visibility == Visibility.Visible);
                Assert.AreEqual(window, UITester.MainWindow);
            });
        }

        [TestMethod]
        public void FindVisualDescendantTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                var dummy = WPFHelper.FindVisualDescendant<Button>(null);
                Assert.IsNull(dummy);

                var treeViewItem = testTreeView.FindVisualDescendant<TreeViewItem>();
                Assert.IsNotNull(treeViewItem);
                Assert.IsInstanceOfType(treeViewItem, typeof(TreeViewItem));
            });
        }

        [TestMethod]
        public void RecursiveContainerFromItemTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                var item = testTreeView.Items[0];
                var treeViewItem = testTreeView.ItemContainerGenerator.RecursiveContainerFromItem(item);
                Assert.IsNotNull(treeViewItem);
                Assert.IsInstanceOfType(treeViewItem, typeof(TreeViewItem));
                ((TreeViewItem)treeViewItem).IsExpanded = true;

                // for code coverage
                item = ((TreeItemViewModel)testTreeView.Items[0]).Children[0];
                treeViewItem = testTreeView.ItemContainerGenerator.RecursiveContainerFromItem(item);
                Assert.IsNull(treeViewItem);
            });
            System.Threading.Thread.Sleep(100);

            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var item = ((TreeItemViewModel)testTreeView.Items[0]).Children[0];
                var treeViewItem = testTreeView.ItemContainerGenerator.RecursiveContainerFromItem(item);
                Assert.IsNotNull(treeViewItem);
                Assert.IsInstanceOfType(treeViewItem, typeof(TreeViewItem));

                treeViewItem = testTreeView.ItemContainerGenerator.RecursiveContainerFromItem(null);
                Assert.IsNull(treeViewItem);
            });
        }

        [TestMethod]
        public void CheckAndInvokeTest()
        {
            bool invoked = false;
            UITester.Dispatcher.CheckAndInvoke(new Action(() => invoked = true));
            Assert.IsTrue(invoked);

            UITester.Dispatcher.Invoke(() => UITester.Dispatcher.CheckAndInvoke(new Action(() => invoked = false)));
            Assert.IsFalse(invoked);

            invoked = UITester.Dispatcher.CheckAndInvoke(() => true);
            Assert.IsTrue(invoked);

            invoked = UITester.Dispatcher.Invoke(() => UITester.Dispatcher.CheckAndInvoke(() => false));
            Assert.IsFalse(invoked);
        }
    }
}
