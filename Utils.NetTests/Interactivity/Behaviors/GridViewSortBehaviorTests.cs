using System.Linq;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.NetTests;

namespace Utils.Net.Interactivity.Behaviors.Tests
{
    [TestClass]
    public class GridViewSortBehaviorTests
    {
        private ListView testListView;
        private GridView testGridView;
        private GridViewSortBehavior testBehavior;

        [TestInitialize]
        public void SetUp()
        {
            UITester.Init(typeof(Utils.Net.Sample.App));

            UITester.Dispatcher.Invoke(() => UITester.Get<ComboBox>().SelectedItem = "ListPage");
            System.Threading.Thread.Sleep(100);

            testListView = UITester.Get<ListView>();
            testGridView = UITester.Dispatcher.Invoke(() => testListView.View as GridView);
            testBehavior = UITester.Dispatcher.Invoke(() => Interaction.GetBehaviors(testListView).OfType<GridViewSortBehavior>().Single());
        }

        [TestMethod]
        public void GridViewSortBehaviorTest()
        {
            // for code coverage
            UITester.Dispatcher.Invoke(() =>
            {
                testBehavior.Detach();

                var header = testGridView.Columns[0].Header as GridViewColumnHeader;
                testGridView.Columns[0].Header = "Test";
                testBehavior.Attach(testListView);
                testBehavior.Detach();

                testGridView.Columns[0].Header = header;
                testBehavior.Attach(testListView);

                header.Command.Execute(header);
                header.Command.Execute(header);
                header = testGridView.Columns[1].Header as GridViewColumnHeader;
                header.Command.Execute(header);
                header.Command.Execute(header);

                testBehavior.Detach();
                testBehavior.Attach(testListView);

                GridViewSortBehavior.SetSortBy(header, header.Content.ToString());
            });
        }
    }
}
