using System;
using System.Collections.Generic;

namespace ShUtilities.Collections
{
    public class TrieKey : IEquatable<TrieKey>
    {
        internal byte[] Indexes { get; }

        internal TrieKey(byte[] indexes)
        {
            Indexes = indexes;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TrieKey);
        }

        public bool Equals(TrieKey other)
        {
            bool result = false;
            if (other != null && Indexes.Length == other.Indexes.Length)
            {
                result = true;
                for (byte i = 0; i < Indexes.Length; i++)
                {
                    if (Indexes[i] != other.Indexes[i])
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        public override int GetHashCode()
        {
            return 831826579 + EqualityComparer<byte[]>.Default.GetHashCode(Indexes);
        }

        public static bool operator ==(TrieKey key1, TrieKey key2)
        {
            return (ReferenceEquals(key1, null) && ReferenceEquals(key2, null)) || (key1.Equals(key2));
        }

        public static bool operator !=(TrieKey key1, TrieKey key2)
        {
            return !(key1 == key2);
        }
    }
}
