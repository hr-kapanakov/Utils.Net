using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.NetTests;

namespace Utils.Net.Controls.Tests
{
    [TestClass]
    public class ToolTipTests
    {
        private ToolTip testToolTip;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            var comboBox = UITester.Get<ComboBox>();
            UITester.Dispatcher.Invoke(() => comboBox.SelectedItem = "ListPage");
            System.Threading.Thread.Sleep(100);
            var textBox = UITester.Get<TextBox>();
            testToolTip = UITester.Dispatcher.Invoke(() => textBox.ToolTip as ToolTip);
            UITester.Dispatcher.Invoke(() => testToolTip.IsOpen = true);
        }

        [TestCleanup]
        public void CleanUp()
        {
            UITester.Dispatcher.Invoke(() => testToolTip.IsOpen = false);
        }

        [TestMethod]
        public void LabelTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var label = testToolTip.Label;
                testToolTip.Label = string.Empty;
                Assert.AreEqual(testToolTip.Label, string.Empty);
                testToolTip.Label = label;
                Assert.AreEqual(testToolTip.Label, label);
            });
        }

        [TestMethod]
        public void HotkeyTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var hotkey = testToolTip.Hotkey;
                testToolTip.Hotkey = string.Empty;
                Assert.AreEqual(testToolTip.Hotkey, string.Empty);
                testToolTip.Hotkey = hotkey;
                Assert.AreEqual(testToolTip.Hotkey, hotkey);
            });
        }

        [TestMethod]
        public void IconSourceTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var iconSource = testToolTip.IconSource;
                testToolTip.IconSource = null;
                Assert.IsNull(testToolTip.IconSource);
                testToolTip.IconSource = iconSource;
                Assert.AreEqual(testToolTip.IconSource, iconSource);
            });
        }

        [TestMethod]
        public void IconWidthTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var iconWidth = testToolTip.IconWidth;
                testToolTip.IconWidth = 24;
                Assert.AreEqual(testToolTip.IconWidth, 24);
                testToolTip.IconWidth = iconWidth;
                Assert.AreEqual(testToolTip.IconWidth, iconWidth);
            });
        }
    }
}
