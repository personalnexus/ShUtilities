using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ShUtilities.Collections;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class ListExtensionsTest
    {
        [TestMethod]
        public void SliceInBounds_InvalidBoundsFullList()
        {
            Span<int> slice = stackalloc int[10];
            CheckSlice(10, Items.SliceInBounds(-1, int.MaxValue, slice), slice, Items);
        }

        [TestMethod]
        public void SliceInBounds_ValidBoundsSubList()
        {
            Span<int> slice = stackalloc int[3];
            CheckSlice(3, Items.SliceInBounds(2, 3, slice), slice, 2, 3, 4);
        }

        [TestMethod]
        public void SliceInBounds_InvalidStart()
        {
            Span<int> slice = stackalloc int[3];
            CheckSlice(2, Items.SliceInBounds(-1, 3, slice), slice, 0, 1);
        }

        [TestMethod]
        public void SliceInBounds_InvalidLength()
        {
            Span<int> slice = stackalloc int[3];
            CheckSlice(2, Items.SliceInBounds(8, 4, slice), slice, 8, 9);
        }

        private static readonly int[] Items = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        private void CheckSlice(int expectedCount, int actualCount, Span<int> slice, params int[] expectedElements)
        {
            Assert.AreEqual(expectedCount, actualCount);
            CollectionAssert.AreEqual(expectedElements, slice.Slice(0, actualCount).ToArray());
        }
    }
}
