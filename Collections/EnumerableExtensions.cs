using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShUtilities.Collections
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns whether the given enumerable does not contains any elements using either the count of a collection or by calling Any()
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            bool result = items is ICollection collection ? collection.Count == 0 : !items.Any();
            return result;
        }

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

        /// <summary>
        /// Returns whether the given enumerable contains at least one item, and if so returns that item as an out-parameter.
        /// </summary>
        public static bool TryFirst<T>(this IEnumerable<T> items, out T item)
        {
            IEnumerator<T> enumerator = items.GetEnumerator();
            bool result = enumerator.MoveNext();
            item = result ? enumerator.Current : default;
            return result;
        }

        /// <summary>
        /// Returns whether the given enumerable contains at least one item matching the given predicate, and if so returns that item as an out-parameter.
        /// </summary>
        public static bool TryFirst<T>(this IEnumerable<T> items, out T item, Predicate<T> predicate)
        {
            item = default;
            bool result = false;
            foreach (T i in items)
            {
                if (predicate(i))
                {
                    item = i;
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
