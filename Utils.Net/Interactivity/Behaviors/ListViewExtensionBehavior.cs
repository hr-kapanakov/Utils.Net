using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Utils.Net.Helpers;

namespace Utils.Net.Interactivity.Behaviors
{
    /// <summary>
    /// Attached functionality extension behavior for <see cref="ListView"/>.
    /// </summary>
    public class ListViewExtensionBehavior : Behavior<ListView>
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="SelectedItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(
                nameof(SelectedItems),
                typeof(INotifyCollectionChanged),
                typeof(ListViewExtensionBehavior),
                new PropertyMetadata(default(INotifyCollectionChanged), OnSelectedItemsChanged));

        /// <summary>
        /// Gets or sets the selected item of the <see cref="TreeView"/>.
        /// </summary>
        public INotifyCollectionChanged SelectedItems
        {
            get => (INotifyCollectionChanged)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsDeselectEnable"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDeselectEnableProperty =
            DependencyProperty.Register(
                nameof(IsDeselectEnable),
                typeof(bool),
                typeof(ListViewExtensionBehavior));

        /// <summary>
        /// Gets or sets a value indicating whether to deselect by clicking over blank space is enabled.
        /// </summary>
        public bool IsDeselectEnable
        {
            get => (bool)GetValue(IsDeselectEnableProperty);
            set => SetValue(IsDeselectEnableProperty, value);
        }

        #endregion


        private bool selectionChangedInProgress = false;


        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
        }


        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectionChangedInProgress || SelectedItems == null)
            {
                return;
            }

            selectionChangedInProgress = true;

            dynamic selectedItems = SelectedItems;

            try
            {
                foreach (var item in e.RemovedItems.Cast<dynamic>().Where(item => selectedItems.Contains(item)))
                {
                    selectedItems.Remove(item);
                }
            }
            catch
            {
            }

            try
            {
                foreach (var item in e.AddedItems.Cast<dynamic>().Where(item => !selectedItems.Contains(item)))
                {
                    selectedItems.Add(item);
                }
            }
            catch
            {
            }

            selectionChangedInProgress = false;
        }

        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsDeselectEnable)
            {
                return;
            }

            var hitTestResult = VisualTreeHelper.HitTest(AssociatedObject, e.GetPosition(AssociatedObject));
            var listViewItem = WPFHelper.FindVisualAncestor<ListViewItem>(hitTestResult?.VisualHit);
            if (listViewItem == null)
            {
                AssociatedObject.SelectedItem = null;
            }
        }


        private static void OnSelectedItemsChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var behavior = target as ListViewExtensionBehavior;

            void handler(object sender, NotifyCollectionChangedEventArgs args)
            {
                if (behavior?.AssociatedObject == null)
                {
                    return;
                }

                var listSelectedItems = behavior.AssociatedObject.SelectedItems;
                if (args.OldItems != null)
                {
                    foreach (var item in args.OldItems)
                    {
                        if (listSelectedItems.Contains(item))
                        {
                            listSelectedItems.Remove(item);
                        }
                    }
                }

                if (args.NewItems != null)
                {
                    foreach (var item in args.NewItems)
                    {
                        if (!listSelectedItems.Contains(item))
                        {
                            listSelectedItems.Add(item);
                        }
                    }
                }

                if (args.Action == NotifyCollectionChangedAction.Reset)
                {
                    listSelectedItems.Clear();
                }
            }

            if (e.OldValue is INotifyCollectionChanged)
            {
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= handler;
            }

            if (e.NewValue is INotifyCollectionChanged)
            {
                (e.NewValue as INotifyCollectionChanged).CollectionChanged += handler;
            }
        }
    }
}
