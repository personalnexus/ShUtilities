using System;
using System.Linq;

namespace ShUtilities.Text
{
    /// <summary>
    /// Convenience methods for common <see cref="string"/> operations
    /// </summary>
    public static class StringExtensions
    {
        public static bool In(this string item, params string[] items)
        {
            bool result = items.Contains(item);
            return result;
        }

        public static string Left(this string text, int length)
        {
            string result = text.Substring(0, length);
            return result;
        }

        public static string Right(this string text, int length)
        {
            string result = text.Substring(text.Length - length, length);
            return result;
        }

        public static string Between(this string text, string left, string right)
        {
            int indexOfLeft = text.IndexOf(left);
            int start = indexOfLeft == -1 ? 0 : indexOfLeft + left.Length;
            int indexOfRight = text.IndexOf(right, start);
            int length = indexOfRight == -1 ? text.Length - start : indexOfRight - start;

            string result = (indexOfLeft == -1 && indexOfRight == -1) ? null : text.Substring(start, length);
            return result;
        }
    }
}
