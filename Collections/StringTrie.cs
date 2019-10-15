using System;
using System.Collections.Generic;

namespace ShUtilities.Collections
{
    public class StringTrie<TValue> : Trie<string, char, TValue>
    {
        private int[] _keyIndexByChar;
        private int[] _currentKeyIndexes;

        public StringTrie(ISet<char> possibleCharacters, int initialCapacity) : base(possibleCharacters, initialCapacity)
        {
            int index = 1;
            foreach (char character in possibleCharacters)
            {
                EnsureMinimumSize(ref _keyIndexByChar, character);
                _keyIndexByChar[character] = index;
                index++;
            }
        }

        protected override Span<int> TranslateKey(string key)
        {
            EnsureMinimumSize(ref _currentKeyIndexes, key.Length);
            for (int i = 0; i < key.Length; i++)
            {
                _currentKeyIndexes[i] = _keyIndexByChar[key[i]];
            }
            return _currentKeyIndexes.AsSpan(0, key.Length);
        }
    }
}
