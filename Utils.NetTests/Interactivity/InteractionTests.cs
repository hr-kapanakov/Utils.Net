using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Net.Interactivity.Behaviors;

namespace Utils.Net.Interactivity.Tests
{
    [TestClass]
    public class InteractionTests
    {
        private DependencyObject testDependencyObject = new DependencyObject();

        private class TestBehavior : Behavior
        {
        }

        public class TestTrigger : Triggers.Trigger
        {
        }


        [TestMethod]
        public void GetBehaviorsTest()
        {
            var behavior = new TestBehavior();
            var behaviors = Interaction.GetBehaviors(testDependencyObject);
            Assert.IsNotNull(behaviors);
            Assert.AreEqual(behaviors.Count, 0);

            behaviors = Interaction.GetBehaviors(testDependencyObject); // for code coverage
            behaviors.Add(behavior);
            Assert.AreEqual(behaviors.Count, 1);
            Assert.AreEqual(behavior.AssociatedObject, testDependencyObject);

            behaviors.Remove(behavior);
            Assert.AreEqual(behaviors.Count, 0);
        }

        [TestMethod]
        public void GetTriggersTest()
        {
            var trigger = new TestTrigger();
            var triggers = Interaction.GetTriggers(testDependencyObject);
            Assert.IsNotNull(triggers);
            Assert.AreEqual(triggers.Count, 0);

            triggers = Interaction.GetTriggers(testDependencyObject); // for code coverage
            triggers.Add(trigger);
            Assert.AreEqual(triggers.Count, 1);
            Assert.AreEqual(trigger.AssociatedObject, testDependencyObject);

            triggers.Remove(trigger);
            Assert.AreEqual(triggers.Count, 0);
        }
    }
}
