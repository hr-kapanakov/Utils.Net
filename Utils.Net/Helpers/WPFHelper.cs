using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Utils.Net.Helpers
{
    /// <summary>
    /// Provides a set of static methods for work with WPF objects.
    /// </summary>
    public static class WPFHelper
    {
        /// <summary>
        /// Try to find first visual ancestor from the specified type of the <see cref="DependencyObject"/>.
        /// </summary>
        /// <typeparam name="T">Type of the searching visual ancestor.</typeparam>
        /// <param name="dependencyObject">Object which ancestor is to be find.</param>
        /// <returns>Visual ancestor of the specified type if found; otherwise null.</returns>
        public static T FindVisualAncestor<T>(this DependencyObject dependencyObject) where T : class
        {
            var target = dependencyObject;

            while (target != null && !(target is T))
            {
                target = VisualTreeHelper.GetParent(target);
            }
            
            return target as T;
        }

        /// <summary>
        /// Recursively search for the <see cref="UIElement"/> container corresponding to the given item.
        /// </summary>
        /// <param name="generator"><see cref="ItemContainerGenerator"/> which will be traced for the set item.</param>
        /// <param name="item">The <see cref="DataObject"/> item to find the <see cref="UIElement"/> for.</param>
        /// <returns>
        /// A System.Windows.UIElement that corresponds to the given item. Returns null if <para/>
        /// the item does not belong to the item collection, or if a System.Windows.UIElement <para/>
        /// has not been generated for it.
        /// </returns>
        public static DependencyObject RecursiveContainerFromItem(this ItemContainerGenerator generator, object item)
        {
            var container = generator.ContainerFromItem(item);
            if (container != null)
            {
                return container;
            }

            for (int i = 0; i < generator.Items.Count; i++)
            {
                if (generator.ContainerFromIndex(i) is ItemsControl itemsControl)
                {
                    var result = itemsControl.ItemContainerGenerator.RecursiveContainerFromItem(item);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
