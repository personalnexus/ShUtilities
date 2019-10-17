using System.Collections.Generic;
using System.Linq;

namespace ShUtilities.Collections
{
    public static class TrieCharacterSets
    {
        public static readonly ISet<char> UpperCaseAndNumbers = Union(Enumerable.Range('A', 26), Enumerable.Range('0', 10));
        public static readonly ISet<char> LowerCaseAndNumbers = Union(Enumerable.Range('a', 26), Enumerable.Range('0', 10));
        public static readonly ISet<char> AlphaNumeric = Union(Enumerable.Range('A', 26), Enumerable.Range('a', 26), Enumerable.Range('0', 10));

        private static ISet<char> Union(params IEnumerable<int>[] enumerables)
        {
            var result = new HashSet<char>();
            foreach (IEnumerable<int> enumerable in enumerables)
            {
                foreach (int i in enumerable)
                {
                    result.Add((char)i);
                }
            }
            return result;
        }
    }
}
