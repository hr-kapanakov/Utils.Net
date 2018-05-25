using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Adorners.Tests
{
    [TestClass]
    public class SortAdornerTests
    {
        [TestMethod]
        public void SortAdornerTest()
        {
            // for code coverage
            var sortAdorner = new SortAdorner(new ListView(), ListSortDirection.Ascending);
        }
    }
}
