using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Utils.Net.ViewModels;

namespace Utils.Net.Sample.ViewModels
{
    public class ListPageViewModel : ViewModelBase
    {
        public class ListViewItem
        {
            public string Column1 { get; set; }
            public string Column2 { get; set; }
            public string Column3 { get; set; }
        }


        public ObservableCollection<ListViewItem> ListViewItems { get; }

        public ICollectionView ListViewItemsSource => CollectionViewSource.GetDefaultView(ListViewItems);

        private string textFilter = string.Empty;
        public string TextFilter
        {
            get => textFilter;
            set
            {
                if (SetPropertyBackingField(ref textFilter, value))
                {
                    ListViewItemsSource.Refresh();
                }
            }
        }

        public IEnumerable<string> AutoCompleteValues => ListViewItems.Select(i => i.Column2);

        public ObservableCollection<CheckableItemViewModel> CheckFilter { get; }

        public string CheckFilterHint => string.Join(", ", CheckFilter.Where(c => c.IsChecked == true));

        public Func<object, string, bool> CheckFilterFilter => (o, s) => ((CheckableItemViewModel)o).Name.ToLower().Contains(s.ToLower());


        public ListPageViewModel()
        {
            ListViewItems = new ObservableCollection<ListViewItem>();
            for (int i = 0; i < 10; i++)
            {
                var lvi = new ListViewItem
                {
                    Column1 = "Column1" + i,
                    Column2 = "Column2" + i,
                    Column3 = "Column3" + i
                };
                ListViewItems.Add(lvi);
            }

            CheckFilter = new ObservableCollection<CheckableItemViewModel>();
            for (int i = 0; i < 10; i++)
            {
                var civm = new CheckableItemViewModel("Column1" + i);
                civm.IsCheckedChanged += (_, __) =>
                {
                    ListViewItemsSource.Refresh();
                    OnPropertyChanged(nameof(CheckFilterHint));
                };
                CheckFilter.Add(civm);
            }

            ListViewItemsSource.Filter = o => Filter(o as ListViewItem);
        }


        private bool Filter(ListViewItem item)
        {
            bool res = item.Column2.ToLower().Contains(TextFilter.ToLower());

            res &= CheckFilter.All(c => c.IsChecked == false) ||
                CheckFilter.Where(c => c.IsChecked == true && c.Name == item.Column1).Any();

            return res;
        }
    }
}
