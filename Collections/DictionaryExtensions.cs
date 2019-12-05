using System;
using System.Collections.Generic;

namespace ShUtilities.Collections
{
    public static class DictionaryExtensions
    { 
        /// <summary>
        /// Compares two dictionaries key by key using the <see cref="EqualityComparer{TValue}.Default"/> for <typeparamref name="TValue"/>.
        /// </summary>
        public static bool Diff<TKey, TValue>(this IDictionary<TKey, TValue> original,
                                              IDictionary<TKey, TValue> other,
                                              out DictionaryDiff<TKey, TValue> diff)
        {
            diff = new DictionaryDiff<TKey, TValue>(original, other);
            return diff.Unchanged.Count != original.Count;
        }

        /// <summary>
        /// Compares two dictionaries key by key using the given <see cref="IEqualityComparer{T}"/> for <typeparamref name="TValue"/>.
        /// </summary>
        public static bool Diff<TKey, TValue>(this IDictionary<TKey, TValue> original, 
                                              IDictionary<TKey, TValue> other, 
                                              IEqualityComparer<TValue> comparer,
                                              out DictionaryDiff<TKey, TValue> diff)
        {
            diff = new DictionaryDiff<TKey, TValue>(original, other, comparer);
            return diff.Unchanged.Count != original.Count;
        }

        /// <summary>
        /// Tries to find a value for the key in the dictionary and failing that under that key adds a new value
        /// using the default constructor
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            if (!dictionary.TryGetValue(key, out TValue result))
            {
                result = new TValue();
                dictionary.Add(key, result);
            }
            return result;
        }

        /// <summary>
        /// Tries to find a value for the key in the dictionary and failing that under that key adds a new value
        /// using the given factory method
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factoryMethod)
        {
            if (!dictionary.TryGetValue(key, out TValue result))
            {
                result = factoryMethod(key);
                dictionary.Add(key, result);
            }
            return result;
        }

        /// <summary>
        /// Tries to find a value for the key in the dictionary and failing that under that key adds a new value 
        /// using the given factory method passing through additional state
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue, TFactoryMethodState>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TFactoryMethodState, TValue> factoryMethod, TFactoryMethodState factoryMethodState)
        {
            if (!dictionary.TryGetValue(key, out TValue result))
            {
                result = factoryMethod(key, factoryMethodState);
                dictionary.Add(key, result);
            }
            return result;
        }
    }
}
