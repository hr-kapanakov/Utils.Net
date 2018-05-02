using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Interactivity.Behaviors.Tests
{
    [TestClass]
    public class GridViewResizeBehaviorTests
    {
        private ListView testListView;
        private GridView testGridView;
        private Behavior testBehavior;

        [TestInitialize]
        public void SetUp()
        {
            testListView = new ListView();
            testGridView  = new GridView();
            testGridView.Columns.Add(new GridViewColumn { Header = "1" });
            testGridView.Columns.Add(new GridViewColumn { Header = "2" });
            testGridView.Columns.Add(new GridViewColumn { Header = "3" });
            ScrollViewer.SetVerticalScrollBarVisibility(testGridView, ScrollBarVisibility.Hidden);
            testListView.View = testGridView;

            testBehavior = new GridViewResizeBehavior();
            testBehavior.Attach(testListView);
        }

        [TestCleanup]
        public void CleanUp()
        {
            testBehavior.Detach();
        }

        [TestMethod]
        public void PercentageWidthTest()
        {
            GridViewResizeBehavior.SetWidth(testGridView.Columns[0], "*");
            GridViewResizeBehavior.SetWidth(testGridView.Columns[1], "10");
            GridViewResizeBehavior.SetWidth(testGridView.Columns[2], "3*");

            testListView.Arrange(new Rect(0, 0, 100 + 10 + 5, 100));
            testListView.UpdateLayout();
            Assert.AreEqual(testGridView.Columns[0].Width, 25);
            Assert.AreEqual(testGridView.Columns[1].Width, 10);
            Assert.AreEqual(testGridView.Columns[2].Width, 75);
        }

        [TestMethod]
        public void MinWidthTest()
        {
            GridViewResizeBehavior.SetWidth(testGridView.Columns[0], "10");
            GridViewResizeBehavior.SetMinWidth(testGridView.Columns[0], 20);

            testListView.Arrange(new Rect(0, 0, 100 + 10 + 5, 100));
            testListView.UpdateLayout();
            Assert.AreEqual(testGridView.Columns[0].Width, 20);
        }

        [TestMethod]
        public void MaxWidthTest()
        {
            GridViewResizeBehavior.SetWidth(testGridView.Columns[0], "10");
            GridViewResizeBehavior.SetMaxWidth(testGridView.Columns[0], 5);

            testListView.Arrange(new Rect(0, 0, 100 + 10 + 5, 100));
            testListView.UpdateLayout();
            Assert.AreEqual(testGridView.Columns[0].Width, 5);
        }
    }
}
