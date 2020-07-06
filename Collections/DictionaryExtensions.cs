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

        /// <summary>
        /// Adds the value only if the key doesn't already exist in the dictionary.
        /// </summary>
        /// <returns>True, if the value was added. False, if the key was already present</returns>
        public static bool AddWithoutOverride<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) => AddWithoutOverride(dictionary, key, value, out _);

        // <summary>
        /// Adds the value only if the key doesn't already exist in the dictionary, returning the previous value to the caller.
        /// </summary>
        /// <returns>True, if the value was added. False, if the key was already present</returns>
        public static bool AddWithoutOverride<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, out TValue previousValue)
        {
            bool result = !dictionary.TryGetValue(key, out previousValue);
            if (result)
            {
                dictionary.Add(key, value);
            }
            return result;
        }

        // .NET 4

        ///// <summary>
        ///// Tries to get a value for the given key and returns the given default value when the key is not found.
        ///// </summary>
        //public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        //{
        //    return GetValueOrDefault(source, key, default);
        //}

        ///// <summary>
        ///// Tries to get a value for the given key and returns the given default value when the key is not found.
        ///// </summary>
        //public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        //{
        //    if (!source.TryGetValue(key, out TValue result))
        //    {
        //        result = defaultValue;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Combines calls to TryGetValue and Remove in a convenience method to obtain the previous value for a removed key
        ///// </summary>
        //public static bool Remove<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, out TValue previousValue)
        //{
        //    bool result = source.TryGetValue(key, out previousValue);
        //    if (result)
        //    {
        //        source.Remove(key);
        //    }
        //    return result;
        //}
    }
}
