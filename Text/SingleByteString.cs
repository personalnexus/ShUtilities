namespace ShUtilities.Text
{
    public struct SingleByteString
    {
        private readonly SingleByteStringStorage _storage;
        private int _start;
        public int Length { get; }

        internal SingleByteString(SingleByteStringStorage storage, int start, int length)
        {
            _storage = storage;
            _start = start;
            Length = length;
        }

        public static implicit operator string(SingleByteString sbs) => sbs.ToString();

        public override string ToString() => _storage.Get(_start, Length);

    }
}
