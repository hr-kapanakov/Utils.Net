using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Helpers;
using Utils.NetTests;

namespace Utils.Net.Controls.Tests
{
    [TestClass]
    public class VirtualizingWrapPanelTests
    {
        private VirtualizingWrapPanel testVirtualizingWrapPanel;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            UITester.Dispatcher.Invoke(() => UITester.Get<ComboBox>().SelectedItem = "ExplorerPage");
            System.Threading.Thread.Sleep(100);

            testVirtualizingWrapPanel = UITester.Get<VirtualizingWrapPanel>();
        }

        [TestMethod]
        public void ItemWidthTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var itemWidth = testVirtualizingWrapPanel.ItemWidth;
                testVirtualizingWrapPanel.ItemWidth = 32;
                Assert.AreEqual(testVirtualizingWrapPanel.ItemWidth, 32);
                testVirtualizingWrapPanel.ItemWidth = itemWidth;
                Assert.AreEqual(testVirtualizingWrapPanel.ItemWidth, itemWidth);
            });
        }

        [TestMethod]
        public void ItemHeightTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var itemHeight = testVirtualizingWrapPanel.ItemHeight;
                testVirtualizingWrapPanel.ItemHeight = 32;
                Assert.AreEqual(testVirtualizingWrapPanel.ItemHeight, 32);
                testVirtualizingWrapPanel.ItemHeight = itemHeight;
                Assert.AreEqual(testVirtualizingWrapPanel.ItemHeight, itemHeight);
            });
        }

        [TestMethod]
        public void OrientationTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var orientation = testVirtualizingWrapPanel.Orientation;
                testVirtualizingWrapPanel.Orientation = Orientation.Vertical;
                Assert.AreEqual(testVirtualizingWrapPanel.Orientation, Orientation.Vertical);
                testVirtualizingWrapPanel.Orientation = orientation;
                Assert.AreEqual(testVirtualizingWrapPanel.Orientation, orientation);
            });
        }

        [TestMethod]
        public void LineUpTest()
        {
            SetVerticalOffsetTest(-SystemParameters.ScrollHeight, () => testVirtualizingWrapPanel.LineUp());
        }

        [TestMethod]
        public void LineDownTest()
        {
            SetVerticalOffsetTest(SystemParameters.ScrollHeight, () => testVirtualizingWrapPanel.LineDown());
        }

        [TestMethod]
        public void LineLeftTest()
        {
            SetHorizontalOffsetTest(-SystemParameters.ScrollWidth, () => testVirtualizingWrapPanel.LineLeft());
        }

        [TestMethod]
        public void LineRightTest()
        {
            SetHorizontalOffsetTest(SystemParameters.ScrollWidth, () => testVirtualizingWrapPanel.LineRight());
        }

        [TestMethod]
        public void PageUpTest()
        {
            SetVerticalOffsetTest(-testVirtualizingWrapPanel.ViewportHeight, () => testVirtualizingWrapPanel.PageUp());
        }

        [TestMethod]
        public void PageDownTest()
        {
            SetVerticalOffsetTest(testVirtualizingWrapPanel.ViewportHeight, () => testVirtualizingWrapPanel.PageDown());
        }

        [TestMethod]
        public void PageLeftTest()
        {
            SetHorizontalOffsetTest(-testVirtualizingWrapPanel.ViewportWidth, () => testVirtualizingWrapPanel.PageLeft());
        }

        [TestMethod]
        public void PageRightTest()
        {
            SetHorizontalOffsetTest(testVirtualizingWrapPanel.ViewportWidth, () => testVirtualizingWrapPanel.PageRight());
        }

        [TestMethod]
        public void MouseWheelUpTest()
        {
            SetVerticalOffsetTest(
                -(SystemParameters.ScrollHeight * SystemParameters.WheelScrollLines),
                () => testVirtualizingWrapPanel.MouseWheelUp());
        }

        [TestMethod]
        public void MouseWheelDownTest()
        {
            SetVerticalOffsetTest(
                SystemParameters.ScrollHeight * SystemParameters.WheelScrollLines,
                () => testVirtualizingWrapPanel.MouseWheelDown());
        }

        [TestMethod]
        public void MouseWheelLeftTest()
        {
            SetHorizontalOffsetTest(
                -(SystemParameters.ScrollWidth * SystemParameters.WheelScrollLines),
                () => testVirtualizingWrapPanel.MouseWheelLeft());
        }

        [TestMethod]
        public void MouseWheelRightTest()
        {
            SetHorizontalOffsetTest(
                SystemParameters.ScrollWidth * SystemParameters.WheelScrollLines,
                () => testVirtualizingWrapPanel.MouseWheelRight());
        }


        private void SetVerticalOffsetTest(double offset, Action action)
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var verticalOffset = testVirtualizingWrapPanel.VerticalOffset;

                action.Invoke();

                Assert.IsTrue(
                    testVirtualizingWrapPanel.VerticalOffset == Math.Max(0.0, verticalOffset + offset) ||
                    testVirtualizingWrapPanel.VerticalOffset ==
                        Math.Min(testVirtualizingWrapPanel.ExtentHeight, verticalOffset + offset) ||
                    testVirtualizingWrapPanel.HorizontalOffset == 0.0);
            });
        }

        private void SetHorizontalOffsetTest(double offset, Action action)
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var horizontalOffset = testVirtualizingWrapPanel.HorizontalOffset;

                action.Invoke();

                Assert.IsTrue(
                    testVirtualizingWrapPanel.HorizontalOffset == Math.Max(0.0, horizontalOffset + offset) ||
                    testVirtualizingWrapPanel.HorizontalOffset ==
                        Math.Min(testVirtualizingWrapPanel.ExtentWidth, horizontalOffset + offset) ||
                    testVirtualizingWrapPanel.HorizontalOffset == 0.0);
            });
        }


        [TestMethod]
        public void MakeVisibleTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var itemsControl = ItemsControl.GetItemsOwner(testVirtualizingWrapPanel);

                var container = itemsControl.FindVisualDescendant<ListViewItem>();
                if (container != null)
                {
                    testVirtualizingWrapPanel.MakeVisible(null, Rect.Empty);
                    testVirtualizingWrapPanel.MakeVisible(container as System.Windows.Media.Visual, Rect.Empty);
                }
            });
        }
    }
}
