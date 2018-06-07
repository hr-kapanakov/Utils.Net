using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Helpers;
using Utils.NetTests;

namespace Utils.Net.Dialogs.Tests
{
    [TestClass]
    public class ProgressDialogTests
    {
        private ProgressDialog testProgressDialog;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));
            testProgressDialog = UITester.Dispatcher.Invoke(() => ProgressDialog.Show("Test", 100));
        }

        [TestCleanup]
        public void CleanUp()
        {
            UITester.Dispatcher.Invoke(() => ((Window)testProgressDialog.Parent).Close());
        }

        [TestMethod]
        public void MessageTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var message = testProgressDialog.Message;
                testProgressDialog.Message = "Test";
                Assert.AreEqual(testProgressDialog.Message, "Test");
                testProgressDialog.Message = message;
                Assert.AreEqual(testProgressDialog.Message, message);
            });
        }

        [TestMethod]
        public void MaximumTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var maximum = testProgressDialog.Maximum;
                testProgressDialog.Maximum = 0;
                Assert.AreEqual(testProgressDialog.Maximum, 0);
                testProgressDialog.Maximum = maximum;
                Assert.AreEqual(testProgressDialog.Maximum, maximum);
            });
        }

        [TestMethod]
        public void CurrentTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var current = testProgressDialog.Current;
                testProgressDialog.Current = 1.0;
                Assert.AreEqual(testProgressDialog.Current, 1.0);
                testProgressDialog.Current = current;
                Assert.AreEqual(testProgressDialog.Current, current);
            });
        }

        [TestMethod]
        public void SubMessageTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var subMessage = testProgressDialog.SubMessage;
                testProgressDialog.SubMessage = "Test";
                Assert.AreEqual(testProgressDialog.SubMessage, "Test");
                testProgressDialog.SubMessage = subMessage;
                Assert.AreEqual(testProgressDialog.SubMessage, subMessage);
            });
        }

        [TestMethod]
        public void EscKeyTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                bool closed = false;
                ((Window)testProgressDialog.Parent).Closed += (_, __) => closed = true;

                testProgressDialog.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Escape)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    });

                Assert.IsTrue(closed);

                // for code coverage
                testProgressDialog.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    });
            });
        }

        [TestMethod]
        public void CancelButtonTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                bool closed = false;
                ((Window)testProgressDialog.Parent).Closed += (_, __) => closed = true;

                var cancelButton = testProgressDialog.FindVisualDescendant<Button>(b => b.Name.Contains("CancelButton"));
                cancelButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, cancelButton));

                Assert.IsTrue(closed);
            });
        }

        [TestMethod]
        public void UpdateProgressTest()
        {
            testProgressDialog.UpdateProgress(10, "Test");
            UITester.Dispatcher.Invoke(() =>
            {
                Assert.AreEqual(testProgressDialog.Current, 10);
                Assert.AreEqual(testProgressDialog.SubMessage, "Test");
            });

            testProgressDialog.UpdateProgress(10, 20, "Test");
            UITester.Dispatcher.Invoke(() =>
            {
                Assert.AreEqual(testProgressDialog.Current, 10);
                Assert.AreEqual(testProgressDialog.Maximum, 20);
                Assert.AreEqual(testProgressDialog.SubMessage, "Test");
            });
        }

        [TestMethod]
        public void DisposeTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                bool closed = false;
                ((Window)testProgressDialog.Parent).Closed += (_, __) => closed = true;

                testProgressDialog.Dispose();

                Assert.IsTrue(closed);
            });
        }
    }
}
