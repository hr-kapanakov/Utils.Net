using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Interactivity.Triggers.Tests
{
    [TestClass]
    public class EventTriggerTests
    {
        private class TestTriggerAction : TriggerActions.TriggerAction
        {
            public bool Invoked { get; private set; } = false;

            protected override void Invoke(object parameter)
            {
                Invoked = true;
            }
        }


        [TestMethod]
        public void EventTriggerTest()
        {
            var textBox = new TextBox();
            var eventTrigger = new EventTrigger(nameof(textBox.TextChanged));
            var action = new TestTriggerAction();

            eventTrigger.Actions.Add(action);
            eventTrigger.Attach(textBox);
            textBox.RaiseEvent(new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.None));
            Assert.IsTrue(action.Invoked);
        }
    }
}