using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Common;
using Utils.Net.Interactivity.Triggers;

namespace Utils.Net.Interactivity.TriggerActions.Tests
{
    [TestClass]
    public class InvokeCommandActionTests
    {
        [TestMethod]
        public void InvokeCommandActionTest()
        {
            var textBox = new TextBox();
            var eventTrigger = new EventTrigger(nameof(textBox.TextChanged));
            var action = new InvokeCommandAction();

            bool invoked = false;
            action.Command = new RelayCommand<bool>(b => invoked = b);
            action.CommandParameter = true;

            eventTrigger.Actions.Add(action);
            eventTrigger.Attach(textBox);
            textBox.RaiseEvent(new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.None));
            Assert.IsTrue(invoked);

            // for code coverage
            action = new InvokeCommandAction("test1")
            {
                Command = null,
                CommandName = "test"
            };
            eventTrigger.Actions.Add(action);
            Assert.AreEqual(action.CommandName, "test");
            textBox.RaiseEvent(new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.None));
        }
    }
}
