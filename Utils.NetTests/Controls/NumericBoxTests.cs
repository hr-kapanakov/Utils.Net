using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Helpers;
using Utils.NetTests;

namespace Utils.Net.Controls.Tests
{
    [TestClass]
    public class NumericBoxTests
    {
        private NumericBox testNumericBox;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            var comboBox = UITester.Get<ComboBox>();
            UITester.Dispatcher.Invoke(() => comboBox.SelectedItem = "OthersPage");
            System.Threading.Thread.Sleep(100);
            testNumericBox = UITester.Get<NumericBox>();
        }

        [TestMethod]
        public void CornerRadiusTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                testNumericBox.CornerRadius = new CornerRadius(3);
                Assert.AreEqual(testNumericBox.CornerRadius, new CornerRadius(3));
            });
        }

        [TestMethod]
        public void StringFormatTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                testNumericBox.StringFormat = "F";
                Assert.AreEqual(testNumericBox.StringFormat, "F");
            });
        }

        [TestMethod]
        public void IsValidTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                ((System.Windows.Controls.TextBox)testNumericBox).Text = "1.";
                ((System.Windows.Controls.TextBox)testNumericBox).Text = "ab";
                Assert.IsFalse(testNumericBox.IsValid);
            });
        }

        [TestMethod]
        public void MinimumTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                testNumericBox.Minimum = 1;
                testNumericBox.Value = 0;
                Assert.AreEqual(testNumericBox.Value, 1);
            });
        }

        [TestMethod]
        public void MaximumTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                testNumericBox.Maximum = 10;
                testNumericBox.Value = 11;
                Assert.AreEqual(testNumericBox.Value, 10);
            });
        }

        [TestMethod]
        public void StepTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                testNumericBox.Step = 0.1;
                Assert.AreEqual(testNumericBox.Step, 0.1);
            });
        }

        [TestMethod]
        public void MouseWheelTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                var value = testNumericBox.Value;

                testNumericBox.RaiseEvent(
                    new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, Mouse.MouseWheelDeltaForOneLine)
                    {
                        RoutedEvent = Mouse.MouseWheelEvent
                    });

                Assert.AreEqual(testNumericBox.Value, value + testNumericBox.Step);
            });
        }

        [TestMethod]
        public void KeyUpTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                var value = testNumericBox.Value;
                testNumericBox.Focus();

                testNumericBox.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Up)
                    {
                        RoutedEvent = Keyboard.KeyUpEvent
                    });
                Assert.AreEqual(testNumericBox.Value, value + testNumericBox.Step);

                testNumericBox.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Down)
                    {
                        RoutedEvent = Keyboard.KeyUpEvent
                    });
                Assert.AreEqual(testNumericBox.Value, value);
            });
        }

        [TestMethod]
        public void ButtonsTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                var value = testNumericBox.Value;

                var upButton = testNumericBox.FindVisualDescendant<Button>(b => b.Name.Contains("UpButton"));
                upButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, upButton));
                Assert.AreEqual(testNumericBox.Value, value + testNumericBox.Step);

                var downButton = testNumericBox.FindVisualDescendant<Button>(b => b.Name.Contains("DownButton"));
                downButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, upButton));
                Assert.AreEqual(testNumericBox.Value, value);
            });
        }
    }
}
