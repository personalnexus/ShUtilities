using System;
using System.Collections.Generic;
using System.Text;

namespace ShUtilities.Collections
{
    public static class CollectionExtensions
    {
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

        public static ISet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            var result = new HashSet<T>(items);
            return result;
        }

        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                set.Add(item);
            }
        }
    }
}
