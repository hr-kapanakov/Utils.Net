using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Helpers;
using Utils.NetTests;

namespace Utils.Net.Dialogs.Tests
{
    [TestClass]
    public class TutorialDialogTests
    {
        private TutorialDialog testTutorialDialog;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));
            UITester.Dispatcher.Invoke(() =>
            {
                testTutorialDialog = new TutorialDialog(new Managers.TutorialManager(), "Title", "Text")
                {
                    PreviousButtonText = "Previous",
                    NextButtonText = "Next",
                    IsCloseButtonVisible = true,
                    IsCheckboxVisible = true
                };

                var window = new Window()
                {
                    Content = testTutorialDialog,
                    ResizeMode = ResizeMode.NoResize,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    Width = 300,
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
            UITester.Dispatcher.Invoke(() => ((Window)testTutorialDialog.Parent).Close());
        }


        [TestMethod]
        public void PreviousButtonTextTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var text = testTutorialDialog.PreviousButtonText;
                testTutorialDialog.PreviousButtonText = "Test";
                Assert.AreEqual(testTutorialDialog.PreviousButtonText, "Test");
                testTutorialDialog.PreviousButtonText = text;
                Assert.AreEqual(testTutorialDialog.PreviousButtonText, text);
            });
        }

        [TestMethod]
        public void NextButtonTextTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var text = testTutorialDialog.NextButtonText;
                testTutorialDialog.NextButtonText = "Test";
                Assert.AreEqual(testTutorialDialog.NextButtonText, "Test");
                testTutorialDialog.NextButtonText = text;
                Assert.AreEqual(testTutorialDialog.NextButtonText, text);
            });
        }

        [TestMethod]
        public void IsCloseButtonVisibleTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var visible = testTutorialDialog.IsCloseButtonVisible;
                testTutorialDialog.IsCloseButtonVisible = false;
                Assert.IsFalse(testTutorialDialog.IsCloseButtonVisible);
                testTutorialDialog.IsCloseButtonVisible = visible;
                Assert.AreEqual(testTutorialDialog.IsCloseButtonVisible, visible);
            });
        }

        [TestMethod]
        public void IsCheckboxVisibleTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var visible = testTutorialDialog.IsCheckboxVisible;
                testTutorialDialog.IsCheckboxVisible = false;
                Assert.IsFalse(testTutorialDialog.IsCheckboxVisible);
                testTutorialDialog.IsCheckboxVisible = visible;
                Assert.AreEqual(testTutorialDialog.IsCheckboxVisible, visible);
            });
        }


        [TestMethod]
        public void OnPreviewKeyDownTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                bool stopped = false;
                testTutorialDialog.Manager.Stopped += (_, __) => stopped = true;
                testTutorialDialog.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Escape)
                    {
                        RoutedEvent = Keyboard.PreviewKeyDownEvent
                    });
                Assert.IsTrue(stopped);

                // for code coverage
                testTutorialDialog.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Left)
                    {
                        RoutedEvent = Keyboard.PreviewKeyDownEvent
                    });
                testTutorialDialog.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Right)
                    {
                        RoutedEvent = Keyboard.PreviewKeyDownEvent
                    });
                testTutorialDialog.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter)
                    {
                        RoutedEvent = Keyboard.PreviewKeyDownEvent
                    });
            });
        }

        [TestMethod]
        public void ButtonsTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                bool stopped = false;
                testTutorialDialog.Manager.Stopped += (_, __) => stopped = true;

                var closeButton = testTutorialDialog.FindVisualDescendant<Button>(b => b.Name == "CloseButton");
                closeButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, closeButton));

                Assert.IsTrue(stopped);


                var checkbox = testTutorialDialog.FindVisualDescendant<CheckBox>(b => b.Name == "CheckBox");
                var isChecked = checkbox.IsChecked;
                checkbox.IsChecked = true;
                checkbox.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, checkbox));
                Assert.AreEqual(checkbox.IsChecked, testTutorialDialog.Manager.DontShowAgain);

                checkbox.IsChecked = isChecked;
                checkbox.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, checkbox));
                Assert.AreEqual(checkbox.IsChecked, testTutorialDialog.Manager.DontShowAgain);

                // for code coverage
                var previousButton = testTutorialDialog.FindVisualDescendant<Button>(b => b.Name == "PreviousButton");
                previousButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, previousButton));

                var nextButton = testTutorialDialog.FindVisualDescendant<Button>(b => b.Name == "NextButton");
                nextButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, nextButton));
            });
        }

        [TestMethod]
        public void ShowPopupTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var popup = TutorialDialog.ShowPopup(testTutorialDialog.Manager, "Title", "Text", PlacementMode.Center, UITester.MainWindow);
                Assert.IsInstanceOfType(popup, typeof(Popup));
                Assert.IsTrue(popup.IsOpen);
                popup.IsOpen = false;
            });
        }

        [TestMethod]
        public void ShowBorderTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var popup = TutorialDialog.ShowBorder(UITester.MainWindow, Colors.Red, new Thickness(3));
                Assert.IsInstanceOfType(popup, typeof(Popup));
                Assert.IsTrue(popup.IsOpen);
                popup.IsOpen = false;
            });
        }
    }
}
