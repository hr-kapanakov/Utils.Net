using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Helpers;
using Utils.NetTests;

namespace Utils.Net.Controls.Tests
{
    [TestClass]
    public class TextBoxTests
    {
        private TextBox testTextBox;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));
            
            UITester.Dispatcher.Invoke(() => UITester.Get<ComboBox>().SelectedItem = "ListPage");
            System.Threading.Thread.Sleep(100);

            testTextBox = UITester.Get<TextBox>();
        }

        [TestMethod]
        public void CornerRadiusTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                testTextBox.CornerRadius = new CornerRadius(3);
                Assert.AreEqual(testTextBox.CornerRadius, new CornerRadius(3));
            });
        }

        [TestMethod]
        public void IconSourceTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var iconSource = testTextBox.IconSource;
                testTextBox.IconSource = null;
                Assert.IsNull(testTextBox.IconSource);
                testTextBox.IconSource = iconSource;
                Assert.AreEqual(testTextBox.IconSource, iconSource);
            });
        }

        [TestMethod]
        public void HintTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var hint = testTextBox.Hint;
                testTextBox.Hint = string.Empty;
                Assert.AreEqual(testTextBox.Hint, string.Empty);
                testTextBox.Hint = hint;
                Assert.AreEqual(testTextBox.Hint, hint);
            });
        }

        [TestMethod]
        public void ShowClearButtonTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                var showClearButton = testTextBox.ShowClearButton;
                testTextBox.ShowClearButton = false;
                var clearButton = testTextBox.FindVisualDescendant<System.Windows.Controls.Button>(b => b.Name.Contains("ClearButton"));
                Assert.AreEqual(clearButton.Visibility, Visibility.Collapsed);
                testTextBox.ShowClearButton = showClearButton;
                Assert.AreEqual(testTextBox.ShowClearButton, showClearButton);
            });
        }

        [TestMethod]
        public void ClearButtonTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                testTextBox.Text = "test";
                Assert.AreEqual(testTextBox.Text, "test");

                var clearButton = testTextBox.FindVisualDescendant<System.Windows.Controls.Button>(b => b.Name.Contains("ClearButton"));
                clearButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, clearButton));
                Assert.AreEqual(testTextBox.Text, string.Empty);
            });
        }
    }
}
