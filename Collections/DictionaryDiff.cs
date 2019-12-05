using System;
using System.Collections.Generic;

namespace ShUtilities.Collections
{
    /// <summary>
    /// Compares two dictionaries key by key and stores the added, removed etc. keys and values
    /// </summary>
    public class DictionaryDiff<TKey, TValue>
    {
        private readonly List<KeyValuePair<TKey, TValue>> _addedInternal = new List<KeyValuePair<TKey, TValue>>();
        private readonly List<KeyValuePair<TKey, TValue>> _removedInternal = new List<KeyValuePair<TKey, TValue>>();

        internal string GetDescription()
        {
            string result =          
$@"Added: {Added.Count} 
{Added.ToDelimitedString(Environment.NewLine)}

Removed: {Added.Count} 
{Removed.ToDelimitedString(Environment.NewLine)}

Changed Values: {Changed.Count} 
{Changed.ToDelimitedString(Environment.NewLine)}

Unchanged: {Unchanged.Count} 
{Unchanged.ToDelimitedString(Environment.NewLine)}";
            return result;
        }

        private readonly List<DictionaryValueChange<TKey, TValue>> _changedInternal = new List<DictionaryValueChange<TKey, TValue>>();
        private readonly List<KeyValuePair<TKey, TValue>> _unchangedInternal = new List<KeyValuePair<TKey, TValue>>();

        public DictionaryDiff(IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> other): this(original, other, EqualityComparer<TValue>.Default)
        {
        }

        public DictionaryDiff(IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> other, IEqualityComparer<TValue> comparer)
        {
            foreach (KeyValuePair<TKey, TValue> originalItem in original)
            {
                if (!other.TryGetValue(originalItem.Key, out TValue otherValue))
                {
                    _removedInternal.Add(originalItem);
                }
                else if (comparer.Equals(originalItem.Value, otherValue))
                {
                    _unchangedInternal.Add(originalItem);
                }
                else
                {
                    _changedInternal.Add(new DictionaryValueChange<TKey, TValue>(originalItem, otherValue));
                }
            }
            foreach (KeyValuePair<TKey, TValue> otherItem in other)
            {
                if (!original.ContainsKey(otherItem.Key))
                {
                    _addedInternal.Add(otherItem);
                }
            }
        }

        public IReadOnlyList<KeyValuePair<TKey, TValue>> Added => _addedInternal;
        public IReadOnlyList<KeyValuePair<TKey, TValue>> Removed => _removedInternal;
        public IReadOnlyList<DictionaryValueChange<TKey, TValue>> Changed => _changedInternal;
        public IReadOnlyList<KeyValuePair<TKey, TValue>> Unchanged => _unchangedInternal;
    }
}
