using System;
using System.Collections.Generic;

namespace ShUtilities.Text
{
    internal class SingleByteStringStorageSegment
    {
        private const int HeaderLength = 4; // Size of an integer
        internal const int MaxStringLength = int.MaxValue - HeaderLength;

        private readonly SingleByteStringStorageOptions _options;
        private byte[] _data;
        private int _nextStart;

        public int UsedBytes => _nextStart;

        internal SingleByteStringStorageSegment(SingleByteStringStorageOptions options)
        {
            _data = new byte[options.GrowthIncrement];
            _options = options;
        }

        internal SingleByteString Add(string s, int length)
        {
            var result = new SingleByteString(this, _nextStart);
            int newStart = _nextStart + HeaderLength + length;
            if (_data.Length < newStart)
            {
                Array.Resize(ref _data, newStart + _options.GrowthIncrement);
            }
            // Write header
            WriteHeader(ref _nextStart, length);
            // Write payload
            _options.Encoding.GetBytes(s, 0, length, _data, _nextStart);
            _nextStart = newStart;
            return result;
        }

        internal bool Remove(int start)
        {
            //TODO: change _data in-place
            int length = GetStringLength(start);
            bool result = length > 0;
            if (result)
            {
                WriteHeader(ref start, -length);
            }
            return result;
        }

        internal IEnumerable<string> GetStrings()
        {
            int start = 0;
            while (true)
            {
                int length = GetStringLength(start);
                if (length == 0)
                {
                    break;
                }
                if (length > 0)
                {
                    yield return GetString(start, length);
                }
                start += HeaderLength + Math.Abs(length);
                if (start > _data.Length)
                {
                    break;
                }
            }
        }

        internal string GetString(int start)
        {
            int length = GetStringLength(start);
            if (length < 0)
            {
                throw new ArgumentException($"String has been removed from {nameof(SingleByteStringStorage)}.");
            }
            string result = GetString(start, length);
            return result;
        }

        private string GetString(int start, int length) => _options.Encoding.GetString(_data, start + HeaderLength, length);

        internal int GetStringLength(int start) => BitConverter.ToInt32(_data, start);

        internal int GetStringHashCode(int start)
        {
            unchecked
            {
                int result = 0;
                int end = start + HeaderLength + GetStringLength(start);
                for (int i = start; i < end; i++)
                {
                    result = (result * 31) ^ _data[i];
                }
                return result;
            }
        }

        internal static bool GetStringEquals(SingleByteStringStorageSegment segment1, int start1, SingleByteStringStorageSegment segment2, int start2)
        {
            // Include the header in the comparison, so different length strings are detected early when comparing the length bytes
            int length = HeaderLength + segment1.GetStringLength(start1);
            bool result = true;
            for (int i = 0; i < length; i++)
            {
                if (segment1._data[start1 + i] != 
                    segment2._data[start2 + i])
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        internal bool CanHold(int length) => _nextStart < int.MaxValue - length;

        private void WriteHeader(ref int index, int header)
        {
            _data[index++] = (byte)header;
            _data[index++] = (byte)(header >> 8);
            _data[index++] = (byte)(header >> 16);
            _data[index++] = (byte)(header >> 24);
        }
    }
}