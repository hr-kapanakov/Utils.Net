using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Helpers;
using Utils.NetTests;

namespace Utils.Net.Controls.Tests
{
    [TestClass]
    public class ComboBoxTests
    {
        private ComboBox testComboBox;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            UITester.Dispatcher.Invoke(() => UITester.Get<System.Windows.Controls.ComboBox>().SelectedItem = "ListPage");
            System.Threading.Thread.Sleep(100);

            testComboBox = UITester.Get<ComboBox>(c => c.IsEditable);
        }

        [TestMethod]
        public void CornerRadiusTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                testComboBox.CornerRadius = new CornerRadius(3);
                Assert.AreEqual(testComboBox.CornerRadius, new CornerRadius(3));
            });
        }

        [TestMethod]
        public void IconSourceTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var iconSource = testComboBox.IconSource;
                testComboBox.IconSource = null;
                Assert.IsNull(testComboBox.IconSource);
                testComboBox.IconSource = iconSource;
                Assert.AreEqual(testComboBox.IconSource, iconSource);
            });
        }

        [TestMethod]
        public void HintTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var hint = testComboBox.Hint;
                testComboBox.Hint = string.Empty;
                Assert.AreEqual(testComboBox.Hint, string.Empty);
                testComboBox.Hint = hint;
                Assert.AreEqual(testComboBox.Hint, hint);
            });
        }

        [TestMethod]
        public void FilterTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var filter = testComboBox.Filter;
                testComboBox.Filter = null;
                Assert.IsNull(testComboBox.Filter);
                testComboBox.Filter = filter;
                Assert.AreEqual(testComboBox.Filter, filter);
            });
        }

        [TestMethod]
        public void FocusTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var textBox = testComboBox.FindVisualDescendant<TextBox>();
                Keyboard.Focus(textBox);
                Keyboard.ClearFocus();

                var hint = testComboBox.Hint;
                testComboBox.Hint = null;
                Keyboard.Focus(textBox);
                Keyboard.ClearFocus();
                testComboBox.Hint = hint;
            });
        }

        [TestMethod]
        public void TextTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var textBox = testComboBox.FindVisualDescendant<TextBox>();
                textBox.Text = "column";
                textBox.Text = "column1";
            });
        }
    }
}
