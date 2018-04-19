using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Utils.Net.Common;
using Utils.Net.Helpers;
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

        public RelayCommand SelectLabelCommand { get; }

        public RelayCommand DeselectLabelCommand { get; }


        public ObservableCollection<ListViewItem> ListViewItems { get; }

        public ICollectionView ListViewItemsSource => CollectionViewSource.GetDefaultView(ListViewItems);

        private ListViewItem selectedItem;
        public ListViewItem SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;

                    var labels = TreeRootItem.Children.OfType<TreeItemViewModel<CheckableItemViewModel>>().ToList();
                    for (int i = 0; i < labels.Count; i++)
                    {
                        labels.AddRange(labels[i].Children.OfType<TreeItemViewModel<CheckableItemViewModel>>());
                        if (selectedItem.Labels.Contains(labels[i].Content.Name))
                        {
                            labels[i].Content.IsChecked = true;
                        }
                        else
                        {
                            labels[i].Content.IsChecked = false;
                        }
                    }
                }
            }
        }

        public RelayCommand DeselectItemCommand { get; }


        public ExplorerPageViewModel()
        {
            TreeRootItem = new TreeItemViewModel<CheckableItemViewModel>(new CheckableItemViewModel(string.Empty), null);

            for (int i = 1; i <= 5; i++)
            {
                TreeRootItem.Children.Add(new TreeItemViewModel<CheckableItemViewModel>(new CheckableItemViewModel("Label" + i), TreeRootItem));
            }

            TreeRootItem.Children[0].Children.Add(new TreeItemViewModel<CheckableItemViewModel>(new CheckableItemViewModel("Label11"), TreeRootItem.Children[0]));

            SelectLabelCommand = new RelayCommand<RoutedPropertyChangedEventArgs<object>>(
                o => SelectedLabel = o.NewValue as TreeItemViewModel<CheckableItemViewModel>);

            DeselectLabelCommand = new RelayCommand<MouseButtonEventArgs>(args =>
            {
                var treeViewControl = args.Source as TreeView;

                var hitTestResult = VisualTreeHelper.HitTest(treeViewControl, args.GetPosition(treeViewControl));
                var treeViewItem = hitTestResult.VisualHit.FindVisualAncestor<TreeViewItem>();
                if (treeViewItem == null)
                {
                    var item = (TreeViewItem)treeViewControl.ItemContainerGenerator.RecursiveContainerFromItem(treeViewControl.SelectedItem);
                    if (item != null)
                    {
                        item.IsSelected = false;
                    }
                }
            });


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

            DeselectItemCommand = new RelayCommand<MouseButtonEventArgs>(args =>
            {
                var listViewControl = args.Source as ListView;

                var hitTestResult = VisualTreeHelper.HitTest(listViewControl, args.GetPosition(listViewControl));
                var listViewItem = WPFHelper.FindVisualAncestor<System.Windows.Controls.ListViewItem>(hitTestResult.VisualHit);
                if (listViewItem == null)
                {
                    listViewControl.SelectedItem = null;
                }
            });
        }


        private bool Filter(ListViewItem listViewItem)
        {
            bool res = SelectedLabel == null || listViewItem.Labels.Contains(SelectedLabel.Content.Name);
            return res;
        }
    }
}
