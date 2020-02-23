namespace ShUtilities.Collections
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Fills the given array with new instances of <typeparamref name="T"/>.
        /// </summary>
        public static void FillNew<T>(this System.Array array) where T : new() => FillNew<T>(array, 0, array.Length);

        /// <summary>
        /// Fills the given array with new instances of <typeparamref name="T"/> starting at index <paramref name="start"/>.
        /// </summary>
        public static void FillNew<T>(this System.Array array, int start) where T : new() => FillNew<T>(array, start, array.Length - start);

        /// <summary>
        /// Fills the given array with new instances of <typeparamref name="T"/> starting at index <paramref name="start"/> for <paramref name="count"/> elements.
        /// </summary>
        public static void FillNew<T>(this System.Array array, int start, int count)
            where T : new()
        {
            int end = start + count;
            for (int i = start; i < end; i++)
            {
                array.SetValue(new T(), i);
            }
        }
    }
}
