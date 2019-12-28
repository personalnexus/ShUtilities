using System.Collections.Generic;

namespace ShUtilities.Collections
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Creates a new item using the default constructor and adds it to the collection.
        /// </summary>
        /// <returns>The newly created item</returns>
        public static T AddNew<T>(this ICollection<T> collection)
            where T : new()
        {
            var item = new T();
            collection.Add(item);
            return item;
        }

        /// <summary>
        /// Adds the convenience <see cref="List{T}.AddRange(IEnumerable{T})"/> method to all <see cref="ICollection{T}"/> implementations
        /// </summary>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                collection.Add(item);
            }
        }
    }
}