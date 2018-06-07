using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Helpers;
using Utils.NetTests;

namespace Utils.Net.Dialogs.Tests
{
    [TestClass]
    public class SimpleInputDialogTests
    {
        private SimpleInputDialog testSimpleInputDialog;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));
            UITester.Dispatcher.Invoke(() =>
            {
                testSimpleInputDialog = new SimpleInputDialog();

                var window = new Window()
                {
                    Content = testSimpleInputDialog,
                    ResizeMode = ResizeMode.NoResize,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    MaxWidth = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    WindowStyle = WindowStyle.SingleBorderWindow,
                    ShowInTaskbar = false,
                };
                window.Show();
            });
        }

        [TestCleanup]
        public void CleanUp()
        {
            UITester.Dispatcher.Invoke(() => ((Window)testSimpleInputDialog.Parent).Close());
        }

        [TestMethod]
        public void MessageTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var message = testSimpleInputDialog.Message;
                testSimpleInputDialog.Message = "Test";
                Assert.AreEqual(testSimpleInputDialog.Message, "Test");
                testSimpleInputDialog.Message = message;
                Assert.AreEqual(testSimpleInputDialog.Message, message);
            });
        }

        [TestMethod]
        public void InputTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var input = testSimpleInputDialog.Input;
                testSimpleInputDialog.Input = "Test";
                Assert.AreEqual(testSimpleInputDialog.Input, "Test");
                testSimpleInputDialog.Input = input;
                Assert.AreEqual(testSimpleInputDialog.Input, input);
            });
        }

        [TestMethod]
        public void EscKeyTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                bool closed = false;
                ((Window)testSimpleInputDialog.Parent).Closed += (_, __) => closed = true;

                testSimpleInputDialog.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Escape)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    });

                Assert.IsTrue(closed);

                // for code coverage
                testSimpleInputDialog.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    });
            });
        }

        [TestMethod]
        public void OKButtonTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                bool closed = false;
                ((Window)testSimpleInputDialog.Parent).Closed += (_, __) => closed = true;

                var cancelButton = testSimpleInputDialog.FindVisualDescendant<Button>(b => b.Content.ToString().Contains("OK"));
                cancelButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, cancelButton));

                Assert.IsTrue(closed);
            });
        }

        [TestMethod]
        public void CancelButtonTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                bool closed = false;
                ((Window)testSimpleInputDialog.Parent).Closed += (_, __) => closed = true;

                var cancelButton = testSimpleInputDialog.FindVisualDescendant<Button>(b => b.Content.ToString().Contains("Cancel"));
                cancelButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, cancelButton));

                Assert.IsTrue(closed);
            });
        }
    }
}
