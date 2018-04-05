using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Utils.Net.Adorners;
using Utils.Net.Common;

namespace Utils.Net.Interactivity.Behaviors
{
    /// <summary>
    /// Attached behavior to <see cref="ListView"/> to sort columns of the <see cref="GridView"/>.
    /// </summary>
    public class GridViewSortBehavior : Behavior<ListView>
    {
        private RelayCommand command;
        private static GridViewColumnHeader listViewSortColumn;
        private static SortAdorner listViewSortAdorner;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridViewSortBehavior" /> class.
        /// </summary>
        public GridViewSortBehavior()
        {
            command = new RelayCommand<GridViewColumnHeader>(h => SortColumn(h));
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            var gridView = AssociatedObject.View as GridView;
            if (gridView == null)
            {
                AssociatedObject.Loaded += (_, __) => OnAttached();
                return;
            }
            
            foreach (var column in gridView.Columns)
            {
                if (column.Header is GridViewColumnHeader header)
                {
                    header.Command = command;
                    header.CommandParameter = header;
                }
            }

            MarkSortedColumn(gridView);
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            var gridView = AssociatedObject.View as GridView;
            foreach (var column in gridView.Columns)
            {
                if (column.Header is GridViewColumnHeader header)
                {
                    header.Command = null;
                    header.CommandParameter = null;
                }
            }
        }


        private void MarkSortedColumn(GridView gridView)
        {
            var gridSourceCollection = AssociatedObject.ItemsSource as ICollectionView;
            if (gridSourceCollection == null)
            {
                gridSourceCollection = CollectionViewSource.GetDefaultView(AssociatedObject.ItemsSource);
            }

            var sortDescriptor = gridSourceCollection.SortDescriptions.FirstOrDefault();
            if (sortDescriptor != null)
            {
                var headers = gridView.Columns.Select(c => c.Header).OfType<GridViewColumnHeader>();
                var header = headers.FirstOrDefault(h => GetSortBy(h) == sortDescriptor.PropertyName);
                if (header != null)
                {
                    listViewSortAdorner = new SortAdorner(header, sortDescriptor.Direction);
                    AdornerLayer.GetAdornerLayer(header).Add(listViewSortAdorner);
                    listViewSortColumn = header;
                }
            }
        }

        private void SortColumn(GridViewColumnHeader header)
        {
            var gridSourceCollection = AssociatedObject.ItemsSource as ICollectionView;
            if (gridSourceCollection == null)
            {
                gridSourceCollection = CollectionViewSource.GetDefaultView(AssociatedObject.ItemsSource);
            }

            if (listViewSortColumn != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortColumn).Remove(listViewSortAdorner);
                gridSourceCollection.SortDescriptions.Clear();
            }

            var newDir = ListSortDirection.Ascending;
            if (listViewSortColumn == header && listViewSortAdorner.Direction == newDir)
            {
                newDir = ListSortDirection.Descending;
            }
            
            listViewSortColumn = header;
            listViewSortAdorner = new SortAdorner(listViewSortColumn, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortColumn).Add(listViewSortAdorner);
            
            string sortBy = GetSortBy(header);
            gridSourceCollection.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }


        #region Attached Dependency Properties

        /// <summary>
        /// Identifies the attached SortBy dependency property.
        /// </summary>
        public static readonly DependencyProperty SortByProperty =
            DependencyProperty.RegisterAttached("SortBy", typeof(string), typeof(GridViewSortBehavior));

        /// <summary>
        /// Get value of the <see cref="SortByProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object from which the value will be get.</param>
        /// <returns>Value of the <see cref="SortByProperty"/> dependency property.</returns>
        public static string GetSortBy(DependencyObject obj)
        {
            return (string)obj.GetValue(SortByProperty);
        }

        /// <summary>
        /// Set value to the <see cref="SortByProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object to which the value will be set.</param>
        /// <param name="value">Value which will be set to the <see cref="SortByProperty"/> dependency property.</param>
        public static void SetSortBy(DependencyObject obj, string value)
        {
            obj.SetValue(SortByProperty, value);
        }

        #endregion
    }
}
