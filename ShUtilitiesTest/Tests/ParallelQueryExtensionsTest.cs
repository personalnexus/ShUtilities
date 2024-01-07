using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class ParallelQueryExtensionsTest
    {
        [TestClass]
        public class SelectWhere
        {
            [TestMethod]
            public void SomeInputsAreInvalid_TheyAreDiscardedAndValidInputsAreReturned()
            {
                // Arrange
                var strings = new[] { "1", "2", "bbb", "4", "5K" };
                var discarded = new List<string>();

                // Act
                ParallelQuery<int> selected = strings
                                              .AsParallel()
                                              .SelectWhere<string, int>(int.TryParse, discarded);

                // Assert
                using (new AssertionScope())
                {
                    selected.Should().BeEquivalentTo(new int[] { 1, 2, 4 });
                    discarded.Should().BeEquivalentTo(new string[] { "bbb", "5K" });
                }
            }

            [TestMethod]
            public void AddInputsAreValid_DiscardsAreEmpty()
            {
                // Arrange
                var numbers = new[] { 1, 3, 2, 3 };
                var namesById = new Dictionary<int, string>
                {
                    [1] = "Jane",
                    [2] = "Chris",
                    [3] = "Mo",
                };
                var discarded = new List<int>();

                // Act
                ParallelQuery<string> selected = numbers
                                              .AsParallel()
                                              .SelectWhere<int, string>(namesById.TryGetValue, discarded);

                // Assert
                using (new AssertionScope())
                {
                    selected.Should().BeEquivalentTo(new string[] { "Jane", "Mo", "Chris", "Mo" });
                    discarded.Should().BeEmpty();
                }
            }
        }
    }
}
