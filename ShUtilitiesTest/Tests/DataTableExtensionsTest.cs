using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShUtilities.Collections;
using FluentAssertions.Execution;
using FluentAssertions;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class DataTableExtensionsTest
    {
        [TestMethod]
        public void ToDataTable_ValuesSelectedIntoAnonymousType_DataTableCreated()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            DataTable expectedTable = CreateExpectedDataTable(readOnly: false);

            // Act
            DataTable table = items
                .Select(x => new 
                { 
                    Value = x, 
                    Name = x.ToString(),
                    Half = x/2.0 
                })
                .ToDataTable();

            // Assert
            table.Should().BeEquivalentTo(expectedTable);
        }

        [TestMethod]
        public void ToDataTable_RequestReadOnly_DataTableCreatedReadOnly()
        {
            // Arrange
            var items = new[] { 1, 2, 3 };
            DataTable expectedTable = CreateExpectedDataTable(readOnly: true);

            // Act
            DataTable table = items
                .Select(x => new
                {
                    Value = x,
                    Name = x.ToString(),
                    Half = x / 2.0
                })
                .ToDataTable(readOnly: true);

            // Assert
            table.Should().BeEquivalentTo(expectedTable);
        }

        private static DataTable CreateExpectedDataTable(bool readOnly)
        {
            DataTable result = new();
            result.Columns.Add("Half", typeof(double)).ReadOnly = readOnly;
            result.Columns.Add("Name", typeof(string)).ReadOnly = readOnly;
            result.Columns.Add("Value", typeof(int)).ReadOnly = readOnly;

            result.Rows.Add(1, "1", 1 / 2.0);
            result.Rows.Add(2, "2", 2 / 2.0);
            result.Rows.Add(3, "3", 3 / 2.0);

            return result;
        }
    }
}
