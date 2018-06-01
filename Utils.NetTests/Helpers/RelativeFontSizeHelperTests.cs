using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Helpers.Tests
{
    [TestClass]
    public class RelativeFontSizeHelperTests
    {
        private readonly TextBlock testTextBlock = new TextBlock();

        [TestMethod]
        public void RelativeFontSizeTests()
        {
            var fontSize = testTextBlock.FontSize;
            RelativeFontSizeHelper.SetRelativeFontSize(testTextBlock, 2);
            Assert.AreEqual(testTextBlock.FontSize, fontSize + RelativeFontSizeHelper.GetRelativeFontSize(testTextBlock));

            // for code coverage
            RelativeFontSizeHelper.SetRelativeFontSize(new DependencyObject(), 2);
        }
    }
}
