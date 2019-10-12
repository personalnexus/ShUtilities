using System;
using System.Collections.Generic;

namespace ShUtilities.Text
{
    /// <summary>
    /// Given a dictionary this class allows getting values from it and applying a parser/converter to them
    /// </summary>
    public class DictionaryParser<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _source;

        /// <summary>
        /// A parser for the special case that TValue = TOutput and no parsing is neccessary
        /// </summary>
        public static bool IdentityParser(TValue input, out TValue output)
        {
            output = input;
            return true;
        }

        public DictionaryParser(IDictionary<TKey, TValue> source)
        {
            _source = source;
        }

        /// <summary>
        /// Gets a value for the given key using the <see cref="IdentityParser(TValue, out TValue)"/> and throws an exception when the key is not found.
        /// </summary>
        public TValue GetValue(TKey key)
        {
            TValue result = GetValue<TValue>(key, IdentityParser);
            return result;
        }

        /// <summary>
        /// Gets a value for the given key and parses it. Throws an exception when the key is not found or the parser fails.
        /// </summary>
        public TOutput GetValue<TOutput>(TKey key, Parser<TValue, TOutput> parser)
        {
            switch (TryGetValueEx(key, out TOutput result, parser))
            {
                case DictionaryParserResult.KeyNotFound: throw new KeyNotFoundException($"Key '{key}' not found.");
                case DictionaryParserResult.ParserFailed: throw new InvalidOperationException($"Cannot convert '{key}' to '{typeof(TOutput)}'.");
            }
            return result;
        }

        /// <summary>
        /// Gets a value for the given key and parses it. Returns default(TOutput) when the key is not found or the parser fails.
        /// </summary>
        public TOutput GetValueOrDefault<TOutput>(TKey key, Parser<TValue, TOutput> parser)
        {
            TOutput result = GetValueOrDefault(key, parser, default(TOutput));
            return result;
        }

        /// <summary>
        /// Gets a value for the given key and parses it. Returns the gioven default output when the key is not found or the parser fails.
        /// </summary>
        public TOutput GetValueOrDefault<TOutput>(TKey key, Parser<TValue, TOutput> parser, TOutput defaultOutput)
        {
            if (!TryGetValue(key, out TOutput result, parser))
            {
                result = defaultOutput;
            }
            return result;
        }

        /// <summary>
        /// Gets a value for the given key and parses it. Returns null for a nullable value type when the key is not found or the parser fails.
        /// </summary>
        public TOutput? GetValueOrNull<TOutput>(TKey key, Parser<TValue, TOutput> parser)
            where TOutput : struct
        {
            TOutput? result;
            if (TryGetValue(key, out TOutput output, parser))
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
        /// Tries to get a value for the given key using the <see cref="IdentityParser(TValue, out TValue)"/>. Returns false when the key is not found.
        /// </summary>
        public bool TryGetValue(TKey key, out TValue output)
        {
            bool result = TryGetValueEx(key, out output, IdentityParser) == DictionaryParserResult.Success;
            return result;
        }

        /// <summary>
        /// Tries to get a value for the given key and parse it. Returns false when the key is not found or the parser fails.
        /// </summary>
        public bool TryGetValue<TOutput>(TKey key, out TOutput output, Parser<TValue, TOutput> parser)
        {
            bool result = TryGetValueEx(key, out output, parser) == DictionaryParserResult.Success;
            return result;
        }

        /// <summary>
        /// Tries to get a value for the given key and parse it. Returns an enumeration indicating whether the key was not found or the parser failed.
        /// </summary>
        public DictionaryParserResult TryGetValueEx<TOutput>(TKey key, out TOutput output, Parser<TValue, TOutput> parser)
        {
            output = default(TOutput);
            DictionaryParserResult result;
            if (!_source.TryGetValue(key, out TValue value))
            {
                result = DictionaryParserResult.KeyNotFound;
            }
            else if (!parser(value, out output))
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

    /// <summary>
    /// One implementation for the most common key and value type combination: string+string
    /// </summary>
    public class DictionaryParser : DictionaryParser<string, string>
    {
        public DictionaryParser(IDictionary<string, string> source) : base(source)
        {
        }
    }
}
