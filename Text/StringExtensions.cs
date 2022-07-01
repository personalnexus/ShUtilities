using System;
using System.Linq;

namespace ShUtilities.Text
{
    /// <summary>
    /// Convenience methods for common <see cref="string"/> operations
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Same as string.IsNullOrEmpty() but as an extension method, can be called on the instance to be checked even when it is null.
        /// </summary>
        public static bool IsNullOrEmpty(this string item) => string.IsNullOrEmpty(item);

        /// <summary>
        /// Returns true if the string instance the method is called on is equal to one of the given <paramref name="items"/>.
        /// </summary>
        /// <remarks>Allows for a compact syntax at the expense of creating an array for potentially every call.</remarks>
        /// <example>
        /// if (value.In("Value1", "OtherValue2"))
        /// {
        ///     Console.Write("It's one of them.")
        /// }
        /// </example>
        public static bool In(this string item, params string[] items)
        {
            bool result = items.Contains(item);
            return result;
        }

        /// <summary>
        /// Returns true if the string instance the method is called on is equal to one of the given <paramref name="items"/>
        /// using the given comparer.
        /// </summary>
        public static bool In(this string item, StringComparer comparer, params string[] items)
        {
            bool result = items.Contains(item, comparer);
            return result;
        }

        /// <summary>
        /// Returns the first <paramref name="count"/> from the beginning of <paramref name="text"/>.
        /// </summary>
        public static string Left(this string text, int count)
        {
            string result = text[..count];
            return result;
        }

        /// <summary>
        /// Returns <paramref name="count"/> from the end of <paramref name="text"/>.
        /// </summary>
        public static string Right(this string text, int count)
        {
            string result = text.Substring(text.Length - count, count);
            return result;
        }

        /// <summary>
        /// Returns the text between the first occurence of <paramref name="left"/> and the following occurence of <paramref name="right"/> in <paramref name="text"/>.
        /// </summary>
        public static string Between(this string text, string left, string right)
        {
            int indexOfLeft = text.IndexOf(left);
            int start = indexOfLeft == -1 ? 0 : indexOfLeft + left.Length;
            int indexOfRight = text.IndexOf(right, start);
            int length = indexOfRight == -1 ? text.Length - start : indexOfRight - start;

            string result = (indexOfLeft == -1 && indexOfRight == -1) ? null : text.Substring(start, length);
            return result;
        }

        /// <summary>
        /// Not intended for public use. See https://twitter.com/personalnexus/status/1520447669843513344 for context.
        /// </summary>
        public static int Length(this string s)
        {
            int length = 0;
            while (true)
            {
                try
                {
                    char dummy = s[length];
                    length++;
                }
                catch
                {
                    break;
                }
            }
            return length;
        }
    }
}
