namespace ShUtilities.Collections
{
    /// <summary>
    /// Reusable lookup element to save the name to index lookup when getting a value.
    /// </summary>
    public readonly ref struct NamedLookupElement<T>
    {
        private readonly NamedLookup _lookup;
        private readonly int _index;

        internal NamedLookupElement(NamedLookup lookup, int index)
        {
            _lookup = lookup;
            _index = index;
        }

        public T Get() => _lookup.GetValue<T>(_index);

        public static implicit operator T(NamedLookupElement<T> element) => element.Get();
    }
}
