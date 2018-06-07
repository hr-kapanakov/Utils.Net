using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.NetTests;

namespace Utils.Net.Interactivity.Behaviors.Tests
{
    [TestClass]
    public class AutoCompleteBehaviorTests
    {
        private readonly List<string> testAutoCompleteList = new List<string>() { "bc", "bcd", "abcd", "abcde" };

        private TextBox testTextBox;
        private AutoCompleteBehavior testBehavior;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));
            
            UITester.Dispatcher.Invoke(() => UITester.Get<ComboBox>().SelectedItem = "ListPage");
            System.Threading.Thread.Sleep(100);

            testTextBox = UITester.Get<TextBox>();

            UITester.Dispatcher.Invoke(() =>
            {
                testBehavior = new AutoCompleteBehavior
                {
                    ItemsSource = testAutoCompleteList,
                    StringComparison = System.StringComparison.InvariantCultureIgnoreCase
                };
                testBehavior.Attach(testTextBox);
            });
        }

        [TestCleanup]
        public void CleanUp()
        {
            testBehavior.Detach();
            UITester.Dispatcher.Invoke(() => testTextBox.Text = string.Empty);
        }

        [TestMethod]
        public void AutoCompleteTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                var testString = "ab";
                testTextBox.Text = testString;

                CollectionAssert.Contains(testAutoCompleteList, testTextBox.Text);
                CollectionAssert.Contains(testAutoCompleteList, testString + testTextBox.SelectedText);

                // for code coverage
                testTextBox.Text = testString.Substring(0, 1);
                testTextBox.Text = string.Empty;

                testBehavior.ItemsSource = null;
                testTextBox.Text = testString;
            });
        }

        [TestMethod]
        public void SetAutoCompleteValueTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                testTextBox.Text = "ab";

                testTextBox.Focus();
                testTextBox.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter)
                    {
                        RoutedEvent = Keyboard.PreviewKeyDownEvent
                    });
                CollectionAssert.Contains(testAutoCompleteList, testTextBox.Text);
                Assert.IsTrue(string.IsNullOrEmpty(testTextBox.SelectedText));

                // for code coverage
                testTextBox.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Escape)
                    {
                        RoutedEvent = Keyboard.PreviewKeyDownEvent,
                    });
                testTextBox.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter)
                    {
                        RoutedEvent = Keyboard.PreviewKeyDownEvent,
                        Source = testBehavior
                    });
            });
        }
    }
}
