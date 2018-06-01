using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Interactivity.Tests
{
    [TestClass]
    public class AttachedObjectTests
    {
        private readonly DependencyObject testDependencyObject = new DependencyObject();

        private class TestAttachedObject : AttachedObject
        {
        }


        [TestMethod]
        public void AttachTest()
        {
            var testAttachedObject = new TestAttachedObject();
            testAttachedObject.Attach(testDependencyObject);
            Assert.AreEqual(testAttachedObject.AssociatedObject, testDependencyObject);

            testAttachedObject.Attach(testDependencyObject); // for code coverage
            Assert.ThrowsException<InvalidOperationException>(() => testAttachedObject.Attach(null));
            testAttachedObject.Clone(); // for code coverage
        }

        [TestMethod]
        public void DetachTest()
        {
            var testAttachedObject = new TestAttachedObject();
            testAttachedObject.Attach(testDependencyObject);
            testAttachedObject.Detach();
            Assert.IsNull(testAttachedObject.AssociatedObject);
        }
    }
}
