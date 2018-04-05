﻿using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Interactivity.Behaviors;

namespace Utils.Net.Interactivity.Behaviors.Tests
{
    [TestClass]
    public class AutoCompleteBehaviorTests
    {
        private TextBox testTextBox;
        private List<string> testAutoCompleteList = new List<string>() { "bc", "bcd", "abcd", "abcde" };

        [TestInitialize]
        public void SetUp()
        {
            testTextBox = new TextBox();

            AutoCompleteBehavior behavior = new AutoCompleteBehavior
            {
                ItemsSource = testAutoCompleteList,
                StringComparison = System.StringComparison.InvariantCultureIgnoreCase
            };
            behavior.Attach(testTextBox);
        }

        [TestMethod]
        public void AutoCompleteTest()
        {
            var testString = "ab";
            testTextBox.Text = testString;

            CollectionAssert.Contains(testAutoCompleteList, testTextBox.Text);
            CollectionAssert.Contains(testAutoCompleteList, testString + testTextBox.SelectedText);
        }

        [TestMethod]
        public void SetAutoCompleteValueTest()
        {
            /*
            TestTextBox.Text = "ab";

            TestTextBox.RaiseEvent(
                new KeyEventArgs(
                    Keyboard.PrimaryDevice,
                    new System.Windows.Interop.HwndSource(0, 0, 0, 0, 0, "", System.IntPtr.Zero),
                    0,
                    Key.Enter)
                {
                    RoutedEvent = Keyboard.PreviewKeyDownEvent
                });
            CollectionAssert.Contains(TestAutoCompleteList, TestTextBox.Text);
            Assert.IsTrue(string.IsNullOrEmpty(TestTextBox.SelectedText));
            //*/
        }
    }
}