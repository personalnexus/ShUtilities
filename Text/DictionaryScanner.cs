using System.Collections.Generic;

namespace ShUtilities.Text
{
    public static class DictionaryScanner
    {
        /// <summary>
        /// Creates a <see cref="Dictionary{string,string}"/> from the string representation of the dictionary. Supports any combination of \r and \n as line breaks.
        /// </summary>
        public static Dictionary<string, string> Parse(string input)
        {
            var result = new Dictionary<string, string>();

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

                result.Add(key, value);

                // Skip over any remaining CRLF
                while (index < length && IsCrLF(input[index]))
                {
                    index++;
                };
            }
            return result;
        }

        private static bool IsCrLF(char c) => c == '\r' || c == '\n';
    }
}
