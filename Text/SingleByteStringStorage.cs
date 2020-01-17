using System;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilities.Text
{
    public class SingleByteStringStorage
    {
        public SingleByteStringStorage() : this(new SingleByteStringStorageOptions())
        {
        }

        public SingleByteStringStorage(SingleByteStringStorageOptions options)
        {
            if (!options.Encoding.IsSingleByte)
            {
                throw new ArgumentException($"{nameof(SingleByteStringStorage)} only works with single byte encodings");
            }
            Options = options;
            CreateNewCurrentSegment();
        }

        private readonly List<SingleByteStringStorageSegment> _segments = new List<SingleByteStringStorageSegment>();
        private SingleByteStringStorageSegment _currentSegment;

        public int Count { get; private set; }

        public int TotalUsedBytes => _segments.Sum(x => x.UsedBytes);

        public SingleByteStringStorageOptions Options { get; }

        public SingleByteString Add(string s)
        {
            SingleByteString result;
            int length = s.Length;
            if (length > SingleByteStringStorageSegment.MaxStringLength)
            {
                throw new ArgumentException($"{nameof(SingleByteStringStorage)} cannot hold individual strings longer than {SingleByteStringStorageSegment.MaxStringLength}.");
            } 
            else if (length == 0)
            {
                result = new SingleByteString();
            }
            else
            {
                if (!_currentSegment.CanHold(length))
                {
                    CreateNewCurrentSegment();
                }
                result = _currentSegment.Add(s, length);
                Count++;
            }
            return result;
        }

        public bool Remove(SingleByteString sbs)
        {
            bool result = sbs.Segment.Remove(sbs.Start);
            if (result)
            {
                Count--;
            }
            return result;
        }

        public IEnumerable<string> GetStrings() => _segments.SelectMany(x => x.GetStrings());

        private void CreateNewCurrentSegment()
        {
            _currentSegment = new SingleByteStringStorageSegment(Options);
            _segments.Add(_currentSegment);
        }
    }
}
