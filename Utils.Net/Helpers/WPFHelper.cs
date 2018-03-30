using System.Windows;
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
        public static T FindVisualAncestor<T>(DependencyObject dependencyObject) where T : class
        {
            var target = dependencyObject;

            while (target != null && !(target is T))
            {
                target = VisualTreeHelper.GetParent(target);
            }
            
            return target as T;
        }
    }
}
