using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.NetTests;

namespace Utils.Net.Controls.Tests
{
    [TestClass]
    public class ButtonTests
    {
        private Button testButton;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            UITester.Dispatcher.Invoke(() => UITester.Get<ComboBox>().SelectedItem = "OthersPage");
            System.Threading.Thread.Sleep(100);

            testButton = UITester.Get<Button>();
        }

        [TestMethod]
        public void CornerRadiusTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                testButton.CornerRadius = new CornerRadius(3);
                Assert.AreEqual(testButton.CornerRadius, new CornerRadius(3));
            });
        }
    }
}
