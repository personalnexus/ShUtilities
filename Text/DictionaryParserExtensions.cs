using System;
using System.Collections.Generic;

namespace ShUtilities.Text
{
    /// <summary>
    /// Given a dictionary this class allows getting values from it and applying a parser/converter to them
    /// </summary>
    public static class DictionaryParserExtensions
    {
        /// <summary>
        /// A parser for the special case that TValue = TOutput and no parsing is neccessary
        /// </summary>
        public static bool IdentityParser<TValue>(TValue input, out TValue output)
        {
            output = input;
            return true;
        }

        /// <summary>
        /// Gets a value for the given key using the <see cref="IdentityParser(TValue, out TValue)"/> and throws an exception when the key is not found.
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            TValue result = GetValue<TKey, TValue, TValue>(source, key, IdentityParser);
            return result;
        }

        /// <summary>
        /// Gets a value for the given key and parses it. Throws an exception when the key is not found or the parser fails.
        /// </summary>
        public static TOutput GetValue<TKey, TValue, TOutput>(this IDictionary<TKey, TValue> source, TKey key, Parser<TValue, TOutput> parser)
        {
            switch (TryGetValueEx(source, key, out TValue rawValue, out TOutput result, parser))
            {
                case DictionaryParserResult.KeyNotFound: throw new KeyNotFoundException($"Key '{key}' not found.");
                case DictionaryParserResult.ParserFailed: throw new InvalidOperationException($"Cannot parse value '{rawValue}' of key '{key}' into '{typeof(TOutput)}'.");
            }
            return result;
        }

        /// <summary>
        /// Gets a value for the given key and parses it. Returns default(TOutput) when the key is not found or the parser fails.
        /// </summary>
        public static TOutput GetValueOrDefault<TKey, TValue, TOutput>(this IDictionary<TKey, TValue> source, TKey key, Parser<TValue, TOutput> parser)
        {
            TOutput result = GetValueOrDefault(source, key, parser, default);
            return result;
        }

        /// <summary>
        /// Gets a value for the given key and parses it. Returns the gioven default output when the key is not found or the parser fails.
        /// </summary>
        public static TOutput GetValueOrDefault<TKey, TValue, TOutput>(this IDictionary<TKey, TValue> source, TKey key, Parser<TValue, TOutput> parser, TOutput defaultOutput)
        {
            if (!TryGetValue(source, key, out TOutput result, parser))
            {
                result = defaultOutput;
            }
            return result;
        }

        /// <summary>
        /// Gets a value for the given key and parses it. Returns null for a nullable value type when the key is not found or the parser fails.
        /// </summary>
        public static TOutput? GetValueOrNull<TKey, TValue, TOutput>(this IDictionary<TKey, TValue> source, TKey key, Parser<TValue, TOutput> parser)
            where TOutput : struct
        {
            TOutput? result;
            if (TryGetValue(source, key, out TOutput output, parser))
            {
                result = output;
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Tries to get a value for the given key and parse it. Returns false when the key is not found or the parser fails.
        /// </summary>
        public static bool TryGetValue<TKey, TValue, TOutput>(this IDictionary<TKey, TValue> source, TKey key, out TOutput output, Parser<TValue, TOutput> parser)
        {
            bool result = TryGetValueEx(source, key, out output, parser) == DictionaryParserResult.Success;
            return result;
        }

        /// <summary>
        /// Tries to get a value for the given key and parse it. Returns an enumeration indicating whether the key was not found or the parser failed.
        /// </summary>
        public static DictionaryParserResult TryGetValueEx<TKey, TValue, TOutput>(this IDictionary<TKey, TValue> source, TKey key, out TOutput output, Parser<TValue, TOutput> parser)
        {
            DictionaryParserResult result = TryGetValueEx(source, key, out _, out output, parser);
            return result;
        }
            
        private static DictionaryParserResult TryGetValueEx<TKey, TValue, TOutput>(this IDictionary<TKey, TValue> source, TKey key, out TValue rawValue, out TOutput output, Parser<TValue, TOutput> parser)
        {
            output = default;
            DictionaryParserResult result;
            if (!source.TryGetValue(key, out rawValue))
            {
                result = DictionaryParserResult.KeyNotFound;
            }
            else if (!parser(rawValue, out output))
            {
                result = DictionaryParserResult.ParserFailed;
            }
            else
            {
                result = DictionaryParserResult.Success;
            }
            return result;
        }
    }
}
