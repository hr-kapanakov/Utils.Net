using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Utils.Net.Helpers;

namespace Utils.Net.Interactivity.Behaviors
{
    /// <summary>
    /// Attached functionality extension behavior for <see cref="TreeView"/>.
    /// </summary>
    public class TreeViewExtensionBehavior : Behavior<TreeView>
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(TreeViewExtensionBehavior),
                new FrameworkPropertyMetadata(
                    default(object), 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                    OnSelectedItemChanged));

        /// <summary>
        /// Gets or sets the selected item of the <see cref="TreeView"/>.
        /// </summary>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsDeselectEnable"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDeselectEnableProperty =
            DependencyProperty.Register(
                nameof(IsDeselectEnable),
                typeof(bool),
                typeof(TreeViewExtensionBehavior));

        /// <summary>
        /// Gets or sets a value indicating whether to deselect by clicking over blank space is enabled.
        /// </summary>
        public bool IsDeselectEnable
        {
            get => (bool)GetValue(IsDeselectEnableProperty);
            set => SetValue(IsDeselectEnableProperty, value);
        }

        #endregion


        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += AssociatedObject_SelectedItemChanged;
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.SelectedItemChanged -= AssociatedObject_SelectedItemChanged;
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
        }


        private void AssociatedObject_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }

        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsDeselectEnable)
            {
                return;
            }

            var hitTestResult = VisualTreeHelper.HitTest(AssociatedObject, e.GetPosition(AssociatedObject));
            var treeViewItem = hitTestResult?.VisualHit?.FindVisualAncestor<TreeViewItem>();
            if (treeViewItem == null)
            {
                var item = (TreeViewItem)AssociatedObject
                    .ItemContainerGenerator
                    .RecursiveContainerFromItem(AssociatedObject.SelectedItem);
                if (item != null)
                {
                    item.IsSelected = false;
                }
            }
        }


        private static void OnSelectedItemChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var behavior = target as TreeViewExtensionBehavior;
            if (behavior?.AssociatedObject?.ItemContainerGenerator
                .RecursiveContainerFromItem(e.NewValue) is TreeViewItem item)
            {
                item.IsSelected = true;
            }
            else if (behavior != null && behavior.SelectedItem == null &&
                behavior?.AssociatedObject?.ItemContainerGenerator
                .RecursiveContainerFromItem(behavior?.AssociatedObject?.SelectedItem) is TreeViewItem item2)
            {
                item2.IsSelected = false;
            }
        }
    }
}
