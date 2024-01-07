using ShUtilities.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShUtilities.Collections
{
    public static class ParallelQueryExtensions
    {
        /// <summary>
        /// Applies Select and Where in a single step to take advantage of TryParse methods, e.g. taking an array of strings and returning 
        /// all those as ints that successfully passed int.TryParse(). Items that were rejected by <paramref name="selector"/> are added
        /// to <paramref name="discarded"/>.
        /// </summary>
        public static ParallelQuery<TResult> SelectWhere<TSource, TResult>(this ParallelQuery<TSource> items, Parser<TSource, TResult> selector, ICollection<TSource> discarded) => items
                   .Select(input =>
                           {
                               bool passed = selector(input, out TResult output);
                               if (!passed)
                               {
                                   discarded.Add(input);
                               }
                               return new SelectorResult<TResult>(output, passed);
                           })
                   .Where(x => x.Passed)
                   .Select(x => x.Result);

        private record SelectorResult<T>(T Result, bool Passed);
    }
}
