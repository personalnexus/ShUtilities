using System;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilities.Collections
{
    public static class SortedListExtensions
    {
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
    }
}
