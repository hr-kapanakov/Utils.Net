using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.NetTests;

namespace Utils.Net.Controls.Tests
{
    [TestClass]
    public class ExpanderTests
    {
        private Expander testExpander;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            UITester.Dispatcher.Invoke(() => UITester.Get<ComboBox>().SelectedItem = "OthersPage");
            System.Threading.Thread.Sleep(100);

            testExpander = UITester.Get<Expander>();
        }

        [TestMethod]
        public void ToggleButtonTemplateTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                var template = (ControlTemplate)testExpander.FindResource("PlusExpanderToggleButtonTemplate");
                testExpander.ToggleButtonTemplate = template;
                Assert.AreEqual(testExpander.ToggleButtonTemplate, template);
                testExpander.ToggleButtonTemplate = null;
                Assert.IsNull(testExpander.ToggleButtonTemplate);
                testExpander.ToggleButtonTemplate = template;
                Assert.AreEqual(testExpander.ToggleButtonTemplate, template);
            });
        }
    }
}
