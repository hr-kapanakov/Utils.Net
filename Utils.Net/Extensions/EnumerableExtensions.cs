using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Net.Extensions
{
    /// <summary>
    /// Provides a set of static methods for querying objects that implement <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Flattens hierarchical structure of elements into one sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements.</typeparam>
        /// <param name="enumerable">Enumerable to be flatten.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns><see cref="IEnumerable{T}"/> sequence of elements obtain from flattening the hierarchy.</returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> enumerable, Func<T, IEnumerable<T>> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return enumerable.Concat(enumerable.SelectMany(c => selector(c).Flatten(selector)));
        }

        /// <summary>
        /// Returns a subset of the elements with set start index and count.
        /// </summary>
        /// <typeparam name="T">Type of the elements.</typeparam>
        /// <param name="enumerable">Enumerable to get subset of.</param>
        /// <param name="start">Index of the starting element.</param>
        /// <param name="count">Elements count to be added to the subset.</param>
        /// <returns><see cref="IEnumerable{T}"/> subset of the elements</returns>
        public static IEnumerable<T> Range<T>(this IEnumerable<T> enumerable, int start, int count)
        {
            return enumerable.Skip(start).Take(count);
        }

        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements.</typeparam>
        /// <param name="enumerable">Enumerable to performs action over.</param>
        /// <param name="action">The <see cref="Action{T}"/> to perform on each element of the <see cref="IEnumerable{T}"/>.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in enumerable)
            {
                action.Invoke(item);
            }
        }

        /// <summary>
        /// Creates a <see cref="ObservableCollection{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements.</typeparam>
        /// <param name="enumerable">Enumerable collection to be turned into observable collection.</param>
        /// <returns>A <see cref="ObservableCollection{T}"/> that contains elements from the input sequence.</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}
