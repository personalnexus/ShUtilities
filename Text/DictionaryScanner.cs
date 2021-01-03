using System.Collections.Generic;

namespace ShUtilities.Text
{
    /// <summary>
    /// Populates a <see cref="Dictionary{string,string}"/> from the string representation of the dictionary. Supports any combination of \r and \n as line breaks.
    /// </summary>
    public static class DictionaryScanner
    {
        /// <summary>
        /// Populates <paramref name="output"/> with all key-value-pairs found in the <paramref name="input"/> string representation
        /// </summary>
        public static void Parse(string input, IDictionary<string, string> output)
        {
            int index = 0;
            int length = input.Length;

            while (index < length)
            {
                int keyStartIndex = index;
                int keyValueSeparatorIndex = -1;

                while (index < length)
                {
                    char character = input[index];

                    if (character == '=')
                    {
                        if (keyValueSeparatorIndex < 0)
                        { 
                            keyValueSeparatorIndex = index;
                        }
                    }
                    else if (IsCrLF(character))
                    {
                        break;
                    }
                    index++;
                }

                string key;
                string value;

                if (keyValueSeparatorIndex < 0)
                {
                    key = input.Substring(keyStartIndex, index - keyStartIndex);
                    value = "";
                }
                else
                {
                    key = input.Substring(keyStartIndex, keyValueSeparatorIndex - keyStartIndex);
                    value = input.Substring(keyValueSeparatorIndex + 1, index - keyValueSeparatorIndex - 1);
                }

                output.Add(key, value);

                // Skip over any remaining CRLF
                while (index < length && IsCrLF(input[index]))
                {
                    index++;
                };
            }
        }

        /// <summary>
        /// Populates <paramref name="output"/> only with relevant key-value-pairs found in the <paramref name="input"/> string representation
        /// </summary>
        public static void Parse(string input, DictionaryRelevantKeys relevantKeys, IDictionary<string, string> output)
        {
            int index = 0;
            int keyStartIndex = index;
            int length = input.Length;
            int keyNodeIndex = 0;
            int previousKeyNodeIndex;
            // valueCountInInput tracks the number of keys found (in case of duplicate keys, the first
            // value is retained and only counted once)
            int valueCountInInput = 0;
            relevantKeys.Clear();

            while (index < length)
            {
                char character = input[index];
                previousKeyNodeIndex = keyNodeIndex;
                if (relevantKeys.Contains(character, ref keyNodeIndex))
                {
                    index++;
                }
                else if (character == '=')
                {
                    // Skip to end of key-value-pair
                    int keyValueSeparatorIndex = index;
                    do
                    {
                        index++;
                    }
                    while (index < length && !IsCrLF(input[index]));
                    if (relevantKeys.SetValue(previousKeyNodeIndex, ref valueCountInInput))
                    {
                        string key = input.Substring(keyStartIndex, keyValueSeparatorIndex - keyStartIndex);
                        string value = input.Substring(keyValueSeparatorIndex + 1, index - keyValueSeparatorIndex - 1);
                        output[key] = value;
                    }
                    // Ignore the rest when all keys have been found
                    if (valueCountInInput == relevantKeys.Count)
                    {
                        break;
                    }
                    // Skip to beginning of next key
                    keyNodeIndex = 0;
                    do
                    {
                        index++;
                    }
                    while (index < length && IsCrLF(input[index]));
                    keyStartIndex = index;
                }
                else
                {
                    // Not key-value-separator => empty value
                    if (relevantKeys.SetValue(keyNodeIndex, ref valueCountInInput))
                    {
                        string key = input.Substring(keyStartIndex, index - keyStartIndex);
                        output[key] = "";
                    }
                    // Skip to end of key-value-pair
                    do
                    {
                        index++;
                    }
                    while (index < length && !IsCrLF(input[index]));
                    // Skip to beginning of next key
                    keyNodeIndex = 0;
                    do
                    {
                        index++;
                    }
                    while (index < length && IsCrLF(input[index]));
                    keyStartIndex = index;
                }
            }
            // If the last key does not end with a key value separator, record an empty key
            if (relevantKeys.SetValue(keyNodeIndex, ref valueCountInInput))
            {
                string key = input.Substring(keyStartIndex, index - keyStartIndex);
                output[key] = "";
            }
        }

        private static bool IsCrLF(char c) => c == '\r' || c == '\n';
    }
}
