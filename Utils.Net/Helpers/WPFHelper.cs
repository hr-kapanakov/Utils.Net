using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Utils.Net.Common;

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


        /// <summary>
        /// Wrap specific data into <see cref="EventArgs{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the data.</typeparam>
        /// <param name="value">Value of the data.</param>
        /// <returns><see cref="EventArgs{T}"/> object wrapping the data.</returns>
        public static EventArgs<T> ToEventArgs<T>(this T value)
        {
            return new EventArgs<T>(value);
        }


        /// <summary>
        /// Gets all open <see cref="Popup"/>s.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> sequence of all open <see cref="Popup"/>s.</returns>
        public static List<Popup> GetOpenPopups()
        {
            return PresentationSource.CurrentSources
                .OfType<System.Windows.Interop.HwndSource>()
                .Select(h => h.RootVisual)
                .OfType<FrameworkElement>()
                .Select(f => f.Parent)
                .OfType<Popup>()
                .Where(p => p.IsOpen)
                .ToList();
        }


        /// <summary>
        /// Format text in <see cref="TextBlock"/> by adding it as inlines.
        /// </summary>
        /// <remarks>Currently only bold and italic are supported (by adding '*' and '_' respectivily in the text).</remarks>
        /// <param name="textBlock"><see cref="TextBlock"/> which will show the formated text.</param>
        /// <param name="text">Text which will be formeted.</param>
        /// <param name="clear">Clear the <see cref="TextBlock"/> text before adding.</param>
        public static void Format(this TextBlock textBlock, string text, bool clear = false)
        {
            if (clear)
            {
                textBlock.Text = string.Empty;
                textBlock.Inlines.Clear();
            }

            bool bold = false;
            bool italic = false;

            int startIdx = 0;
            for (int i = 0; i < text.Length; i++)
            {
                // skip non special characters
                if (text[i] != '*' && text[i] != '_')
                {
                    continue;
                }

                // if there are two '*'/'_' escape the symbol
                if (i < text.Length - 1 && text[i] == text[i + 1])
                {
                    i++;
                    continue;
                }

                // if there is a text to be added (between special characters)
                if (i - startIdx > 0)
                {
                    var textPart = text.Substring(startIdx, i - startIdx).Replace("**", "*").Replace("__", "_");
                    var run = new Run(textPart)
                    {
                        FontWeight = bold ? FontWeights.Bold : FontWeights.Normal,
                        FontStyle = italic ? FontStyles.Italic : FontStyles.Normal
                    };
                    textBlock.Inlines.Add(run);
                }
                startIdx = i + 1;

                // toggle the style based on character
                if (text[i] == '*')
                {
                    bold = !bold;
                }
                if (text[i] == '_')
                {
                    italic = !italic;
                }
            }

            // add the rest of the text
            if (startIdx < text.Length - 1)
            {
                textBlock.Inlines.Add(new Run(text.Substring(startIdx).Replace("**", "*").Replace("__", "_")));
            }
        }
    }
}
