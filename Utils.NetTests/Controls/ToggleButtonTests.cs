using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.NetTests;

namespace Utils.Net.Controls.Tests
{
    [TestClass]
    public class ToggleButtonTests
    {
        private ToggleButton testToggleButton;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            UITester.Dispatcher.Invoke(() => UITester.Get<ComboBox>().SelectedItem = "OthersPage");
            System.Threading.Thread.Sleep(100);

            testToggleButton = UITester.Get<ToggleButton>();
        }

        [TestMethod]
        public void CornerRadiusTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                testToggleButton.CornerRadius = new CornerRadius(3);
                Assert.AreEqual(testToggleButton.CornerRadius, new CornerRadius(3));
            });
        }
    }
}
