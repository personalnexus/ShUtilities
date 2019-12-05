using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;

namespace ShUtilitiesTest
{
    public static class DictionaryAssert
    {
        public static void AreEqual<TKey, TValue>(IDictionary<TKey, TValue> expected, IDictionary<TKey, TValue> actual)
        {
            bool haveDiffs = expected.Diff(actual, out DictionaryDiff<TKey, TValue> diff);
            if (haveDiffs)
            {
                Assert.Fail("Dictionaries are not equal.\r\n" + diff.GetDescription());
            }
        }
    }
}
