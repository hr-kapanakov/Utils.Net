using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Interactivity.Triggers.Tests
{
    [TestClass]
    public class EventTriggerTests
    {
        private class TestTriggerAction : TriggerActions.TriggerAction<TextBox>
        {
            public bool Invoked { get; private set; } = false;

            protected override void Invoke(object parameter)
            {
                Invoked = true;
                Assert.IsInstanceOfType(parameter, typeof(TextChangedEventArgs));
                Assert.IsInstanceOfType(AssociatedObject, typeof(TextBox));
            }
        }


        [TestMethod]
        public void EventTriggerTest()
        {
            Assert.IsInstanceOfType(new EventTrigger(), typeof(EventTrigger));

            var textBox = new TextBox();
            var eventTrigger = new EventTrigger(nameof(textBox.TextChanged));
            var action = new TestTriggerAction
            {
                IsEnabled = true
            };
            Assert.IsTrue(action.IsEnabled);

            eventTrigger.Actions.Add(action);
            eventTrigger.Attach(textBox);
            textBox.RaiseEvent(new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.None));
            Assert.IsTrue(action.Invoked);
            Assert.ThrowsException<ArgumentException>(() => eventTrigger.EventName = "test");

            // for code coverage
            action.Attach(textBox);
            eventTrigger.Actions.Remove(action);
            eventTrigger.Actions.Add(action);
            eventTrigger.Detach();
        }
    }
}
