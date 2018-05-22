using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

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
        /// <param name="dependencyObject">Object where a predecessor has to be found.</param>
        /// <param name="condition">Optional condition that search element must meet.</param>
        /// <returns>Visual ancestor of the specified type if found; otherwise null.</returns>
        public static T FindVisualAncestor<T>(this DependencyObject dependencyObject, Func<T, bool> condition = null) where T : DependencyObject
        {
            var target = dependencyObject;

            while (target != null)
            {
                target = VisualTreeHelper.GetParent(target);

                if ((target is T result) && (condition == null || condition(result)))
                {
                    break;
                }
            }
            
            return target as T;
        }

        /// <summary>
        /// Try to find first visual descendant from the specified type of the <see cref="DependencyObject"/>.
        /// </summary>
        /// <typeparam name="T">Type of the searching visual descendant.</typeparam>
        /// <param name="dependencyObject">Object on which a descendant must be found.</param>
        /// <param name="condition">Optional condition that search element must meet</param>
        /// <returns>Visual descendant of the specified type if found; otherwise null.</returns>
        public static T FindVisualDescendant<T>(this DependencyObject dependencyObject, Func<T, bool> condition = null) where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                return null;
            }

            var count = VisualTreeHelper.GetChildrenCount(dependencyObject);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);
                if ((child is T result) && (condition == null || condition(result)))
                {
                    return result;
                }

                var next = FindVisualDescendant(child, condition);
                if (next != null)
                {
                    return next;
                }
            }
            return null;
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

        /// <summary>
        /// Executes the specified <see cref="Action"/> synchronously on the thread the set <see cref="Dispatcher"/> is associated with.
        /// </summary>
        /// <param name="dispatcher">Dispatcher on which thread will be executed the set <see cref="Action"/>.</param>
        /// <param name="callback">A delegate to invoke through the dispatcher.</param>
        public static void CheckAndInvoke(this Dispatcher dispatcher, Action callback)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(callback);
            }
            else
            {
                callback.Invoke();
            }
        }

        /// <summary>
        /// Executes the specified <see cref="Func{TResult}"/> synchronously on the thread the set <see cref="Dispatcher"/> is associated with.
        /// </summary>
        /// <typeparam name="T">Type of the expected result.</typeparam>
        /// <param name="dispatcher">Dispatcher on which thread will be executed the set <see cref="Func{TResult}"/>.</param>
        /// <param name="callback">A delegate to invoke through the dispatcher.</param>
        /// <returns>The return value type of the specified delegate</returns>
        public static T CheckAndInvoke<T>(this Dispatcher dispatcher, Func<T> callback)
        {
            if (!dispatcher.CheckAccess())
            {
                return dispatcher.Invoke(callback);
            }
            else
            {
                return callback.Invoke();
            }
        }
    }
}
