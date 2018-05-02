using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.ViewModels.Tests
{
    [TestClass]
    public class CheckableItemViewModelTests
    {
        [TestMethod]
        public void CheckableItemViewModelTest()
        {
            var testCheckableItemViewModel = new CheckableItemViewModel<string>("Name", "Value");

            Assert.AreEqual(testCheckableItemViewModel.Name, "Name");
            testCheckableItemViewModel.Name = "NewName";
            Assert.AreNotEqual(testCheckableItemViewModel.Name, "Name");

            Assert.AreEqual(testCheckableItemViewModel.Value, "Value");
            testCheckableItemViewModel.Value = "NewValue";
            Assert.AreNotEqual(testCheckableItemViewModel.Value, "Value");

            Assert.IsTrue(testCheckableItemViewModel.IsChecked.HasValue);
            Assert.IsFalse(testCheckableItemViewModel.IsChecked.Value);
            testCheckableItemViewModel.IsChecked = true;
            testCheckableItemViewModel.IsCheckedChanged += (_, e) => Assert.AreEqual(e.Value, testCheckableItemViewModel.IsChecked);
            testCheckableItemViewModel.IsChecked = false;
            testCheckableItemViewModel.IsChecked = true;
            testCheckableItemViewModel.IsChecked = true; // for code coverage
            Assert.IsTrue(testCheckableItemViewModel.IsChecked.Value);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var testCheckableItemViewModel = new CheckableItemViewModel("Name");
            var testCheckableItemViewModelOfT = new CheckableItemViewModel<string>("Name", "Value");
            
            Assert.AreEqual(testCheckableItemViewModel.ToString(), "Name");
            Assert.AreEqual(testCheckableItemViewModelOfT.ToString(), "Name: Value");
        }
    }
}
