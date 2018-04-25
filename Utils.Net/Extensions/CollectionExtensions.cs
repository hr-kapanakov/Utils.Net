using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Utils.Net.Extensions
{
    /// <summary>
    /// Provides a set of static methods for querying objects that implement <see cref="Collection{T}"/>.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds the elements of the specified enumerable to the end of the <see cref="Collection{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements.</typeparam>
        /// <param name="collection">The collection at the end of which the elements will be added.</param>
        /// <param name="enumerable">The enumerable whose elements should be added to the end of the <see cref="Collection{T}"/>.</param>
        public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            foreach (var item in enumerable)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Updates a specified <see cref="Collection{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements.</typeparam>
        /// <param name="dist">Collection to be updated.</param>
        /// <param name="source"><see cref="IEnumerable{T}"/> to which elements the destination collection will be updated.</param>
        public static void UpdateCollection<T>(this Collection<T> dist, IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // find elements for add
            var toAdd = source.Except(dist).ToList();
            foreach (var item in toAdd)
            {
                dist.Add(item);
            }

            // find elements for remove
            var forRemove = dist.Except(source).ToList();
            foreach (var item in forRemove)
            {
                dist.Remove(item);
            }
        }
    }
}
