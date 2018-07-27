using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Common;
using Utils.Net.Helpers;
using Utils.Net.Sample.Models;
using Utils.NetTests;

namespace Utils.Net.Dialogs.Tests
{
    [TestClass]
    public class SettingsDialogTests
    {
        private readonly Settings testSettings = new Settings();
        private SettingsDialog testSettingsDialog;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));
            UITester.Dispatcher.Invoke(() =>
            {
                var settings = new Settings();

                var commands = new Dictionary<string, ICommand>
                {
                    {
                        "Browse",
                        new RelayCommand<Settings>(s => s.Name = "OK")
                    }
                };

                testSettingsDialog = new SettingsDialog(new object()); // for code coverage
                testSettingsDialog = new SettingsDialog(testSettings); // for code coverage
                testSettingsDialog = new SettingsDialog(testSettings, commands);

                var window = new Window()
                {
                    Content = testSettingsDialog,
                    ResizeMode = ResizeMode.NoResize,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    MaxWidth = 400,
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
            UITester.Dispatcher.Invoke(() => ((Window)testSettingsDialog.Parent).Close());
        }

        [TestMethod]
        public void GetEditorHandlerTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                testSettingsDialog.GetEditorHandler = GetEditorHandler;
                testSettingsDialog.GetEditorHandler = GetEditorHandler;
                Assert.AreEqual(testSettingsDialog.GetEditorHandler, GetEditorHandler);
            });
        }

        [TestMethod]
        public void ExpandableGroupsTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var expandableGroups = testSettingsDialog.ExpandableGroups;
                testSettingsDialog.ExpandableGroups = true;
                testSettingsDialog.ExpandableGroups = true;
                Assert.AreEqual(testSettingsDialog.ExpandableGroups, true);
                testSettingsDialog.ExpandableGroups = expandableGroups;
                Assert.AreEqual(testSettingsDialog.ExpandableGroups, expandableGroups);
            });
        }

        [TestMethod]
        public void FixedSizeTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var fixedSize = testSettingsDialog.FixedSize;
                testSettingsDialog.FixedSize = false;
                testSettingsDialog.FixedSize = false;
                Assert.AreEqual(testSettingsDialog.FixedSize, false);
                testSettingsDialog.FixedSize = fixedSize;
                Assert.AreEqual(testSettingsDialog.FixedSize, fixedSize);
            });
        }

        [TestMethod]
        public void UpdateSourceTriggerTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var updateSourceTrigger = testSettingsDialog.UpdateSourceTrigger;
                testSettingsDialog.UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.Default;
                testSettingsDialog.UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.Default;
                Assert.AreEqual(testSettingsDialog.UpdateSourceTrigger, System.Windows.Data.UpdateSourceTrigger.Default);
                testSettingsDialog.UpdateSourceTrigger = updateSourceTrigger;
                Assert.AreEqual(testSettingsDialog.UpdateSourceTrigger, updateSourceTrigger);
            });
        }

        [TestMethod]
        public void UpdateDelayTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var updateDelay = testSettingsDialog.UpdateDelay;
                testSettingsDialog.UpdateDelay = 200;
                testSettingsDialog.UpdateDelay = 200;
                Assert.AreEqual(testSettingsDialog.UpdateDelay, 200);
                testSettingsDialog.UpdateDelay = updateDelay;
                Assert.AreEqual(testSettingsDialog.UpdateDelay, updateDelay);
            });
        }

        [TestMethod]
        public void CommandTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                testSettingsDialog.FindVisualDescendant<TabItem>(ti => ti.Header.Equals("Category")).IsSelected = true;
            });
            System.Threading.Thread.Sleep(100);
            UITester.Dispatcher.Invoke(() =>
            {
                    var commandButton = testSettingsDialog
                    .FindVisualDescendant<Button>(b => b.Content != null && b.Content.ToString().Contains("Browse"));
                commandButton.Command.Execute(null);

                Assert.AreEqual(testSettings.Name, "OK");
            });
        }

        [TestMethod]
        public void EscKeyTest()
        {
            UITester.Dispatcher.Invoke(() =>
            {
                bool closed = false;
                ((Window)testSettingsDialog.Parent).Closed += (_, __) => closed = true;

                testSettingsDialog.RaiseEvent(
                    new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Escape)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    });

                Assert.IsTrue(closed);

                // for code coverage
                testSettingsDialog.RaiseEvent(
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
                ((Window)testSettingsDialog.Parent).Closed += (_, __) => closed = true;

                var cancelButton = testSettingsDialog
                    .FindVisualDescendant<Button>(b => b.Content != null && b.Content.ToString().Contains("OK"));
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
                ((Window)testSettingsDialog.Parent).Closed += (_, __) => closed = true;

                var cancelButton = testSettingsDialog
                    .FindVisualDescendant<Button>(b => b.Content != null && b.Content.ToString().Contains("Cancel"));
                cancelButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, cancelButton));

                Assert.IsTrue(closed);
            });
        }


        private Control GetEditorHandler(PropertyInfo property, out DependencyProperty dependencyProperty)
        {
            if (property.PropertyType == typeof(DateTime))
            {
                dependencyProperty = TextBox.TextProperty;
                return new TextBox
                {
                    Padding = new Thickness(0, 2, 0, 2),
                    Background = System.Windows.Media.Brushes.Red,
                    IsReadOnly = !property.CanWrite
                };
            }

            dependencyProperty = null;
            return null;
        }
    }
}
