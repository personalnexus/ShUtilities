using System;
using System.Collections.Generic;
using System.Linq;


namespace ShUtilities.Collections
{
    public static class SortedListExtensions
    {
        /// <summary>
        /// Copies the elements below and above the <paramref name="pivot"/> element in <paramref name="list"/> into the given spans.
        /// If the pivot is contained in the list, it is excluded. The number of desired elments is determined by the lengths of
        /// <paramref name="below"/> and <paramref name="above"/>. The return value tuple indicates how many items were actually
        /// copied into the spans in case there weren't enough in the source list.
        /// </summary>
        public static (int resultBelowCount, int resultAboveCount) GetBelowAndAbove<TKey, TValue>(this SortedList<TKey, TValue> list, TKey pivot, Span<TValue> below, Span<TValue> above)
        {
            int aboveStart = TryGetIndexOf(list.Keys, list.Comparer, pivot, out int pivotIndex) ? pivotIndex + 1 : pivotIndex;
            return (list.Values.SliceInBounds(pivotIndex - below.Length, below.Length, below),
                    list.Values.SliceInBounds(aboveStart, above.Length, above));
        }

        /// <summary>
        /// Tries to return all values from a <see cref="SortedList{TKey, TValue}"/> whose keys fall into the given range.
        /// </summary>
        /// <returns>True, if at least one matching value was found, otherwise false.</returns>
        public static bool TryGetRange<TKey, TValue>(this SortedList<TKey, TValue> list, TKey lowerBound, TKey upperBound, out IEnumerable<TValue> values)
        {
            bool result = TryGetRange(list, list.Values, lowerBound, upperBound, out values);
            return result;
        }

        /// <summary>
        /// Tries to return all key-value-pairs from a <see cref="SortedList{TKey, TValue}"/> whose keys fall into the given range.
        /// </summary>
        /// <returns>True, if at least one matching value was found, otherwise false.</returns>
        public static bool TryGetRange<TKey, TValue>(this SortedList<TKey, TValue> list, TKey lowerBound, TKey upperBound, out IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            bool result = TryGetRange(list, list, lowerBound, upperBound, out keyValuePairs);
            return result;
        }

        private static bool TryGetRange<TKey, TValue, TResult>(this SortedList<TKey, TValue> list, IEnumerable<TResult> resultSourceItems, TKey lowerBound, TKey upperBound, out IEnumerable<TResult> resultItems)
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
            resultItems = result ? resultSourceItems.Skip(lowerIndex).Take(valueCount) : Enumerable.Empty<TResult>();

            return result;
        }

        /// <summary>
        /// Used internally by TryGetRange on a SortedList's keys and therefore assumes the list <paramref name="keys"/> is sorted and does not contain duplicates.
        /// </summary>
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
    }
}
