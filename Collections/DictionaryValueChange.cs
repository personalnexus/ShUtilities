using System.Collections.Generic;

namespace ShUtilities.Collections
{
    /// <summary>
    /// Details from the diff of a dictionary about a change in value in one of the keys
    /// </summary>
    public struct DictionaryValueChange<TKey, TValue>
    {
        public DictionaryValueChange(KeyValuePair<TKey, TValue> oldKeyValuePair, TValue newValue): this(oldKeyValuePair.Key, oldKeyValuePair.Value, newValue)
        {
        }

        public DictionaryValueChange(TKey key, TValue oldValue, TValue newValue)
        {
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public TKey Key { get; }
        public TValue OldValue { get; }
        public TValue NewValue { get; }

        public override string ToString()
        {
            return $"[{Key}, {OldValue} -> {NewValue}]";
        }
    }
}
