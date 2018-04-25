using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Utils.Net.Extensions;
using Utils.Net.ViewModels;

namespace Utils.Net.Sample.ViewModels
{
    public class ExplorerPageViewModel : ViewModelBase
    {
        public class ListViewItem
        {
            public string Name { get; set; }
            public List<string> Labels { get; } = new List<string>();
        }

        public TreeItemViewModel TreeRootItem { get; }

        private TreeItemViewModel<CheckableItemViewModel> selectedLabel;
        public TreeItemViewModel<CheckableItemViewModel> SelectedLabel
        {
            get => selectedLabel;
            set
            {
                if (SetPropertyBackingField(ref selectedLabel, value))
                {
                    ListViewItemsSource.Refresh();
                }
            }
        }


        public ObservableCollection<ListViewItem> ListViewItems { get; }

        public ICollectionView ListViewItemsSource => CollectionViewSource.GetDefaultView(ListViewItems);

        private ListViewItem selectedItem;
        public ListViewItem SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem == value)
                {
                    return;
                }

                selectedItem = value;

                var labels = TreeRootItem.Children.Flatten(c => c.Children).OfType<TreeItemViewModel<CheckableItemViewModel>>();
                labels.ForEach(l => l.Content.IsChecked = false);

                if (selectedItem != null)
                {
                    foreach (var labelName in selectedItem.Labels)
                    {
                        var label = labels.Where(l => l.Content.Name == labelName).SingleOrDefault();
                        if (label != null)
                        {
                            label.Propagate(l => l.Content.IsChecked = true);
                        }
                    }
                }
            }
        }

        public ObservableCollection<ListViewItem> SelectedItems { get; } = new ObservableCollection<ListViewItem>();


        public ExplorerPageViewModel()
        {
            TreeRootItem = new TreeItemViewModel<CheckableItemViewModel>(new CheckableItemViewModel(string.Empty), null);

            for (int i = 1; i <= 5; i++)
            {
                TreeRootItem.Children.Add(new TreeItemViewModel<CheckableItemViewModel>(new CheckableItemViewModel("Label" + i), TreeRootItem));
            }

            TreeRootItem.Children[0].Children.Add(new TreeItemViewModel<CheckableItemViewModel>(new CheckableItemViewModel("Label11"), TreeRootItem.Children[0]));
            

            ListViewItems = new ObservableCollection<ListViewItem>();
            for (int i = 0; i < 10; i++)
            {
                var lvi = new ListViewItem()
                {
                    Name = "Item" + i
                };
                lvi.Labels.Add("Label" + ((i % 5) + 1));
                lvi.Labels.Add("Label" + (((i + 2) % 5) + 1));
                ListViewItems.Add(lvi);
            }

            ListViewItemsSource.SortDescriptions.Add(new SortDescription(nameof(ListViewItem.Name), ListSortDirection.Ascending));
            ListViewItemsSource.Filter = o => Filter(o as ListViewItem);
        }


        private bool Filter(ListViewItem listViewItem)
        {
            bool res = SelectedLabel == null || listViewItem.Labels.Contains(SelectedLabel.Content.Name);
            return res;
        }
    }
}
