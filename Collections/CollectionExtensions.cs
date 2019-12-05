using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace ShUtilities.Collections
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Iterating over all items in the <see cref="IEnumerable{T}"/> concats the string representations for each one 
        /// delimited by the given delimiter live string.Join but in a LINQ-like syntax
        /// </summary>
        public static string ToDelimitedString<T>(this IEnumerable<T> items, string delimiter)
        {
            var resultBuilder = new StringBuilder();
            foreach (T item in items)
            {
                if (resultBuilder.Length != 0)
                {
                    resultBuilder.Append(delimiter);
                }
                resultBuilder.Append(item.ToString());
            }
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Creates a HashSet from the given <see cref="IEnumerable{T}"/>
        /// </summary>
        public static ISet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            var result = new HashSet<T>(items);
            return result;
        }

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

        /// <summary>
        /// Partitions a long input sequence into individual lists of at most <paramref name="itemsPerPartition"/> items.
        /// </summary>
        public static IEnumerable<List<T>> ToPartitions<T>(this IEnumerable<T> items, int itemsPerPartition)
        {
            var partition = new List<T>();
            foreach (T item in items)
            {
                if (partition == null)
                {
                    // When we create a new list, we size it to fit all items we are 
                    // expecting. We can't do this from the start, because one might 
                    // pass in a very large value for itemsPerPartition (expecting to
                    // get all items in one partition) resulting in an out-of-memory
                    // exception when allocating the initial list. But here, we have had
                    // a full partition, giving us confidence, the next one will also fit
                    // into memory.
                    partition = new List<T>(itemsPerPartition);
                }
                partition.Add(item);
                if (partition.Count == itemsPerPartition)
                {
                    yield return partition;
                    partition = null;
                    // Don't allocate a new partition just yet, in case this was the
                    // last item
                }
            }
            if (partition != null && partition.Count != 0)
            {
                yield return partition;
            }
        }

        private static bool TryGetIndexOf<TKey>(IList<TKey> keys, IComparer<TKey> comparer, TKey key, out int index)
        {
            bool result = false;
            index = 0;
            int lowerBound = 0;
            int upperBound = keys.Count - 1;

            while (lowerBound <= upperBound && !result)
            {
                int pivot = lowerBound + (upperBound - lowerBound) / 2;
                int comparisonResult = comparer.Compare(key, keys[pivot]);
                if (comparisonResult == 0)
                {
                    result = true;
                    index = pivot;
                }
                else if (comparisonResult < 0)
                {
                    upperBound = pivot - 1;
                }
                else
                {
                    lowerBound = pivot + 1;
                }
            }
            if (!result)
            {
                index = lowerBound;
            }
            return result;
        }

        /// <summary>
        /// Tries to return all values from a <see cref="SortedList{TKey, TValue}"/> whose keys fall into the given range.
        /// </summary>
        /// <returns>True, if at least one matching value was found, otherwise false.</returns>
        public static bool TryGetRange<TKey, TValue>(this SortedList<TKey, TValue> list, TKey lowerBound, TKey upperBound, out IEnumerable<TValue> values)
        {
            if (list.Comparer.Compare(lowerBound, upperBound) > 0)
            {
                throw new ArgumentException($"{nameof(lowerBound)} must be less than or equal to {nameof(upperBound)}.");
            }

            TryGetIndexOf(list.Keys, list.Comparer, lowerBound, out int lowerIndex);
            int upperIndex = lowerIndex;
            while (upperIndex < list.Count)
            {
                if (list.Comparer.Compare(upperBound, list.Keys[upperIndex]) < 0)
                {
                    break;
                }
                upperIndex++;
            }
            int valueCount = upperIndex - lowerIndex;

            bool result = valueCount != 0;
            values = result ? list.Values.Skip(lowerIndex).Take(valueCount) : Enumerable.Empty<TValue>();

            return result;
        }

        /// <summary>
        /// Returns whether the given enumerable does not contains any elements using either the count of a collection or by calling Any()
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            bool result = items is ICollection collection ? collection.Count == 0 : !items.Any();
            return result;
        }
    }
}
