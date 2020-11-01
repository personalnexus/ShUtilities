using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ShUtilities.Collections;
using static ShUtilitiesTest.ExceptionUtility;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class SortedListExtensionsTest
    {
        [TestMethod]
        public void TryGetBelowAndAbove_PivotNotFound()
        {
            var list = new SortedList<int, int>(Enumerable.Range(0, 10).ToDictionary(x => x, x => x));
            list.Remove(5);
            Span<int> below = stackalloc int[2];
            Span<int> above = stackalloc int[4];
            (int belowCount, int aboveCount) = list.GetBelowAndAbove(5, below, above);
            CollectionAssert.AreEqual(new[] { 3, 4 }, below.ToArray());
            CollectionAssert.AreEqual(new[] { 6, 7, 8, 9 }, above.ToArray());
        }

        [TestMethod]
        public void TryGetBelowAndAbove_PivotFound()
        {
            var list = new SortedList<int, int>(Enumerable.Range(0, 10).ToDictionary(x => x, x => x));
            Span<int> below = stackalloc int[2];
            Span<int> above = stackalloc int[4];
            (int belowCount, int aboveCount) = list.GetBelowAndAbove(5, below, above);
            CollectionAssert.AreEqual(new[] { 3, 4 }, below.ToArray());
            CollectionAssert.AreEqual(new[] { 6, 7, 8, 9 }, above.ToArray());
        }

        [TestMethod]
        public void TryGetRange()
        {
            var list = new SortedList<int, int>(Enumerable.Range(0, 1000).ToDictionary(x => x, x => x));
            list.Remove(3);

            Check(2, 2, true, 2);
            Check(2, 3, true, 2);
            Check(3, 4, true, 4);
            Check(2, 4, true, 2, 4);
            Check(2, 6, true, 2, 4, 5, 6);
            Check(-1, 2, true, 0, 1, 2);
            Check(999, 1002, true, 999);
            Check(-1, 1002, true, list.Values.ToArray());

            Check(3, 3, false);
            Check(9999, 999999, false);

            Expect<ArgumentException>(() => list.TryGetRange(3, -1, out IEnumerable<int> _));

            void Check(int lowerBound, int upperBound, bool expectedResult, params int[] expectedValues)
            {
                bool actualResult = list.TryGetRange(lowerBound, upperBound, out IEnumerable<int> values);
                Assert.AreEqual(expectedResult, actualResult);
                if (expectedResult)
                {
                    int[] actualValues = values.ToArray();
                    CollectionAssert.AreEqual(expectedValues, actualValues, $"{lowerBound}-{upperBound}: {actualValues.ToDelimitedString(",")}");
                }
            }
        }
    }
}
