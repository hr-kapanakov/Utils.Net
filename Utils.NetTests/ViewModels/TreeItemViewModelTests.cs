using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.ViewModels.Tests
{
    [TestClass]
    public class TreeItemViewModelTests
    {
        [TestMethod]
        public void TreeItemViewModelTest()
        {
            var testTreeItemViewModel = new TreeItemViewModel<string>("Root");

            Assert.IsFalse(testTreeItemViewModel.IsExpanded);
            testTreeItemViewModel.IsExpanded = true;
            testTreeItemViewModel.IsExpandedChanged += (_, e) => Assert.AreEqual(e.Value, testTreeItemViewModel.IsExpanded);
            testTreeItemViewModel.IsExpanded = false;
            testTreeItemViewModel.IsExpanded = true;
            testTreeItemViewModel.IsExpanded = true; // for code coverage
            Assert.IsTrue(testTreeItemViewModel.IsExpanded);
        }

        [TestMethod]
        public void PropagateTest()
        {
            var testTreeItemViewModel = new TreeItemViewModel<string>("Root");
            testTreeItemViewModel.Children.Add(new TreeItemViewModel<string>("Child", testTreeItemViewModel));

            Assert.AreEqual(testTreeItemViewModel.Children.Count, 1);
            Assert.AreEqual(testTreeItemViewModel.Children[0].ParentItem, testTreeItemViewModel);

            testTreeItemViewModel.Propagate(t => t.IsExpanded = true);
            Assert.IsTrue(testTreeItemViewModel.IsExpanded);
            Assert.IsTrue(testTreeItemViewModel.Children[0].IsExpanded);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var testTreeItemViewModel = new TreeItemViewModel<string>("Root");
            testTreeItemViewModel.Children.Add(new TreeItemViewModel<string>("Child", testTreeItemViewModel));
            testTreeItemViewModel.IsExpanded = true;

            Assert.AreEqual(testTreeItemViewModel.ToString(), "-Root");
            Assert.AreEqual(testTreeItemViewModel.Children[0].ToString(), "-Root.+Child");
        }
    }
}
