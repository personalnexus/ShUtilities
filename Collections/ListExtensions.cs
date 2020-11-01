using System;
using System.Collections.Generic;

namespace ShUtilities.Collections
{
    public static class ListExtensions
    {
        /// <summary>
        /// Copies elements from <paramref name="source"/> into the <paramref name="destination"/> after adjusting <paramref name="start"/> and <paramref name="length"/> to fit the source.
        /// </summary>
        public static int SliceInBounds<T>(this IList<T> source, int start, int length, Span<T> destination)
        {
            if (start < 0)
            {
                length = length + start;
                start = 0;
            }
            if (start + length >= source.Count)
            {
                length = source.Count - start;
            }
            int destinationIndex = 0;
            for (int sourceIndex = start; sourceIndex < start + length; sourceIndex++, destinationIndex++)
            {
                destination[destinationIndex] = source[sourceIndex];
            }
            return destinationIndex;
        }
    }
}
