using ShUtilities.Text;
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
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
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
        /// Returns whether the given enumerable contains at least one item matching the given predicate, and if so returns the first item that does as an out-parameter.
        /// </summary>
        public static bool TryFirst<T>(this IEnumerable<T> items, Func<T, bool> predicate, out T item)
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

        /// <summary>
        /// Returns whether the given enumerable contains at least one item, and if so returns the last item as an out-parameter.
        /// </summary>
        public static bool TryLast<T>(this IEnumerable<T> items, out T item)
        {
            item = default;
            bool result = false;
            if (items is IList<T> list)
            {
                int count = list.Count;
                if (count > 0)
                {
                    item = list[count - 1];
                    result = true;
                }
            }
            else
            {
                foreach (T i in items)
                {
                    item = i;
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns whether the given enumerable contains at least one item matching the given predicate, and if so returns the last item that does as an out-parameter.
        /// </summary>
        public static bool TryLast<T>(this IEnumerable<T> items, Func<T, bool> predicate, out T item)
        {
            item = default;
            bool result = false;
            if (items is IList<T> list)
            {
                // For a list, we can easily iterate in reverse, minimizing the number of times predicate is checked
                // Review: Is it a good idea to execute predicate differently when passing in a list vs. any other enumerable?
                int count = list.Count;
                for (int index = count - 1; index >= 0; index--)
                {
                    T i = list[index];
                    if (predicate(i))
                    {
                        item = i;
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                // We could iterate over items.Reverse() but that would potentially need to allocate a large buffer
                foreach (T i in items)
                {
                    if (predicate(i))
                    {
                        item = i;
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Applies Select and Where in a single step to take advantage TryParse methods, e.g. taking an array of strings and returning 
        /// all those as ints that successfully passed int.TryParse()
        /// </summary>
        public static IEnumerable<TResult> SelectWhere<TSource, TResult>(this IEnumerable<TSource> items, Parser<TSource, TResult> selector)
        {
            foreach (TSource item in items)
            {
                if (selector(item, out TResult result))
                {
                    yield return result;
                }
            }
        }
    }
}
