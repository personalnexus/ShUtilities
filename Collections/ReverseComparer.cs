using System;
using System.Collections;
using System.Collections.Generic;

namespace ShUtilities.Collections
{
    /// <summary>
    /// Reverses the comparison result of the given comparer
    /// </summary>
    public class ReverseComparer<T>: IComparer<T>, IComparer
    {
        private readonly IComparer<T> _comparer;

        public readonly static IComparer<T> Default = new ReverseComparer<T>(Comparer<T>.Default);

        public ReverseComparer(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public int Compare(T x, T y) => _comparer.Compare(y, x);

        int IComparer.Compare(object x, object y)
        {
            if (x is T xt && y is T yt)
            {
                return _comparer.Compare(yt, xt);
            }
            else
            {
                throw new ArgumentException($"Both objects to compare must be of type {typeof(T)}");
            }
        }
    }
}
