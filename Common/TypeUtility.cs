using System;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilities.Common
{
    /// <summary>
    /// Helper functionality for working with types
    /// </summary>
    public static class TypeUtility
    {
        /// <summary>
        /// Produces <see cref="Dictionary{TKey, TValue}"/> of the values of an enum indexed by the names of the enum.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, TEnum> GetEnumValuesByName<TEnum>()
        {
            var result = GetEnumNamesAndValues<TEnum>().ToDictionary(x => x.Name, x => x.Value);
            return result;
        }

        /// <summary>
        /// Produces an <see cref="IEnumerable{T}"/> with tuples of names and values of the given enum in a typed, LINQ-friendly fashion.
        /// </summary>
        public static IEnumerable<(string Name, TEnum Value)> GetEnumNamesAndValues<TEnum>()
        {
            string[] names = Enum.GetNames(typeof(TEnum));
            Array values = Enum.GetValues(typeof(TEnum));
            return values.Cast<TEnum>().Select((value, index) => (names[index], value));
        }
    }
}
