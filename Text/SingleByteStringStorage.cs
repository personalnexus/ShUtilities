using System;
using System.Text;

namespace ShUtilities.Text
{
    public class SingleByteStringStorage
    {
        public Encoding _encoding;
        private byte[] _data = new byte[85000];
        private int _nextStart;

        public int GrowthIncrement { get; set; } = 85000;

        public SingleByteStringStorage() : this(Encoding.GetEncoding("ISO-8859-1"))
        {
        }

        public SingleByteStringStorage(Encoding encoding)
        {
            if (!encoding.IsSingleByte)
            {
                throw new ArgumentException("SingleByteStringStorage only works with single byte encodings");
            }
            _encoding = encoding;
        }

        public SingleByteString Add(string s)
        {
            int length = s.Length;
            int newSize = _nextStart + length;
            var result = new SingleByteString(this, _nextStart, length);
            _encoding.GetBytes(s, 0, length, EnsureSize(newSize), _nextStart);
            _nextStart = newSize;
            return result;
        }

        internal string Get(int start, int length) => _encoding.GetString(_data, start, length);

        private byte[] EnsureSize(int newSize)
        {
            if (_data.Length < newSize)
            {
                Array.Resize(ref _data, newSize + GrowthIncrement);
            }
            return _data;
        }
    }
}
