using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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


        [TestMethod]
        public void ToEventArgsTest()
        {
            var eventArgs = "Test".ToEventArgs();
            Assert.IsInstanceOfType(eventArgs, typeof(EventArgs));
            Assert.AreEqual(eventArgs.Value, "Test");
        }


        [TestMethod]
        public void GetOpenPopupsTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                var tutorialManager = ((Sample.MainWindowViewModel)UITester.MainWindow.DataContext).TutorialManager;
                tutorialManager.Start();
                UITester.MainWindow.Activate();

                var popups = WPFHelper.GetOpenPopups();
                Assert.AreEqual(popups.Count, 1);
            });
        }


        [TestMethod]
        public void FormatTest()
        {
            var testTextBlock = new TextBlock();

            var text = "*bold* normal _italic_ *_bold**Italic_* _*italic__Bold*_";
            testTextBlock.Format(text);

            var runs = testTextBlock.Inlines.OfType<Run>().ToList();
            Assert.AreEqual(runs.Count, 7);

            Assert.AreEqual(runs[0].Text, "bold");
            Assert.AreEqual(runs[0].FontWeight, FontWeights.Bold);
            Assert.AreEqual(runs[0].FontStyle, FontStyles.Normal);

            Assert.AreEqual(runs[1].Text, " normal ");
            Assert.AreEqual(runs[1].FontWeight, FontWeights.Normal);
            Assert.AreEqual(runs[1].FontStyle, FontStyles.Normal);

            Assert.AreEqual(runs[2].Text, "italic");
            Assert.AreEqual(runs[2].FontWeight, FontWeights.Normal);
            Assert.AreEqual(runs[2].FontStyle, FontStyles.Italic);

            Assert.AreEqual(runs[3].Text, " ");
            Assert.AreEqual(runs[3].FontWeight, FontWeights.Normal);
            Assert.AreEqual(runs[3].FontStyle, FontStyles.Normal);

            Assert.AreEqual(runs[4].Text, "bold*Italic");
            Assert.AreEqual(runs[4].FontWeight, FontWeights.Bold);
            Assert.AreEqual(runs[4].FontStyle, FontStyles.Italic);

            Assert.AreEqual(runs[5].Text, " ");
            Assert.AreEqual(runs[5].FontWeight, FontWeights.Normal);
            Assert.AreEqual(runs[5].FontStyle, FontStyles.Normal);

            Assert.AreEqual(runs[6].Text, "italic_Bold");
            Assert.AreEqual(runs[6].FontWeight, FontWeights.Bold);
            Assert.AreEqual(runs[6].FontStyle, FontStyles.Italic);


            testTextBlock.Format("normal", true);
            Assert.AreEqual(testTextBlock.Inlines.Count, 1);

            var run = testTextBlock.Inlines.FirstInline as Run;
            Assert.AreEqual(run.Text, "normal");
            Assert.AreEqual(run.FontWeight, FontWeights.Normal);
            Assert.AreEqual(run.FontStyle, FontStyles.Normal);
        }
    }
}
