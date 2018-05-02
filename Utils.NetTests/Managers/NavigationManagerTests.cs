using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Managers.Tests
{
    [TestClass]
    public class NavigationManagerTests
    {
        #region UnitTestVariables

        private const string TestFirstUserControlTitle = "FirstUserControlTitle";
        private const string TestSecondUserControlTitle = "SecondUserControlTitle";

        private NavigationManager navManager;
        private static UserControl testFirstUserControl;
        private static UserControl testSecondUserControl;

        private enum TestType
        {
            /// <summary>
            /// NavigateTo Test
            /// </summary>
            NavigateToTest,
            /// <summary>
            /// NavigateForward Test
            /// </summary>
            NavigateForwardTest,
            /// <summary>
            /// NavigateBackword Test
            /// </summary>
            NavigateBackwordTest
        }

        #endregion


        private void TestSetup(TestType testType)
        {
            // Navigation to an user control
            testFirstUserControl = new UserControl() { DataContext = new DummyUserControlViewModel() { ControlTitle = TestFirstUserControlTitle } };
            navManager.NavigateTo(testFirstUserControl);

            if (testType == TestType.NavigateBackwordTest || testType == TestType.NavigateForwardTest)
            {
                // Navigating to new user control, so we will have one user control in the PageManagers's backward stack 
                testSecondUserControl = new UserControl() { DataContext = new DummyUserControlViewModel() { ControlTitle = TestSecondUserControlTitle } };
                navManager.NavigateTo(testSecondUserControl);

                // Navigating to top of the backward stack, so we will have one user control the PageManagers's forward stack
                navManager.NavigateBackward();
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            navManager = new NavigationManager();
        }

        [TestMethod]
        public void NavigateToTest()
        {
            TestSetup(TestType.NavigateToTest);

            // Init - just test the basic functionality for navigation
            Assert.AreEqual((navManager.CurrentControl.DataContext as DummyUserControlViewModel).ControlTitle, TestFirstUserControlTitle);

            // Navigating to a new user control, so we will have one user control in the PageManagers's backward stack
            testSecondUserControl = new UserControl() { DataContext = new DummyUserControlViewModel() { ControlTitle = TestSecondUserControlTitle } };
            navManager.NavigateTo(testSecondUserControl);
            Assert.AreEqual((navManager.CurrentControl.DataContext as DummyUserControlViewModel).ControlTitle, TestSecondUserControlTitle);

            // Check if skipping navigation to the same user control is working correctly
            Assert.IsFalse(navManager.NavigateTo(testSecondUserControl));
            navManager.NavigateTo(testFirstUserControl);
            Assert.IsTrue(navManager.CanNavigateBackward);
            Assert.IsTrue(navManager.NavigateBackward());
            Assert.IsTrue(navManager.NavigateBackward());
            Assert.IsFalse(navManager.NavigateBackward());
            Assert.IsFalse(navManager.CanNavigateBackward);
            Assert.IsTrue(navManager.CanNavigateForward);
            Assert.AreEqual((navManager.CurrentControl.DataContext as DummyUserControlViewModel).ControlTitle, TestFirstUserControlTitle);
        }

        [TestMethod]
        public void NavigateBackwardTest()
        {
            TestSetup(TestType.NavigateBackwordTest);

            // Testing if navigation backward works correctly
            Assert.IsTrue((navManager.CurrentControl.DataContext as DummyUserControlViewModel).ControlTitle.Equals(TestFirstUserControlTitle));

            // Checking if navigating backward with clearing the PageManager's forward stack works correctly
            navManager.NavigateForward();
            navManager.NavigateBackward(true);
            navManager.NavigateForward();
            Assert.AreEqual((navManager.CurrentControl.DataContext as DummyUserControlViewModel).ControlTitle, TestFirstUserControlTitle);
        }

        [TestMethod]
        public void NavigateForwardTest()
        {
            TestSetup(TestType.NavigateForwardTest);

            navManager.NavigateForward();
            Assert.AreEqual((navManager.CurrentControl.DataContext as DummyUserControlViewModel).ControlTitle, TestSecondUserControlTitle);
        }

        [TestMethod]
        public void RemoveFromBackwardStackTest()
        {
            // for code coverage
            TestSetup(TestType.NavigateForwardTest);

            for (int i = 0; i < navManager.BackwardStackCapacity + 1; i++)
            {
                if (i % 2 == 0)
                {
                    navManager.NavigateTo(testFirstUserControl);
                }
                else
                {
                    navManager.NavigateTo(testSecondUserControl);
                }
            }
        }

        /// <summary>
        /// PageManager test helper class 
        /// </summary>
        private class DummyUserControlViewModel
        {
            /// <summary>
            /// Gets or sets the value of the dummy user control title
            /// </summary>
            public string ControlTitle { get; set; }

            /// <summary>
            /// Overrides the system method for PageManager usage. 
            /// </summary>
            /// <param name="obj">The object to compare with the current object.</param>
            /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                if ((obj as DummyUserControlViewModel)?.ControlTitle == ControlTitle && ControlTitle != null)
                {
                    return true;
                }

                return obj == this;
            }

            /// <summary>
            /// Overrides the default GetHashCode method
            /// </summary>
            /// <returns>Hash code of the object.</returns>
            public override int GetHashCode()
            {
                if (string.IsNullOrEmpty(ControlTitle))
                {
                    return base.GetHashCode();
                }

                return ControlTitle.GetHashCode();
            }
        }
    }
}
