using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Data;
using ShUtilitiesTest.Mocks;
using System.Collections.Generic;
using System.Data;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class DbCommandRepositoryTest
    {
        [TestMethod]
        public void GetDbCommandText()
        {
            var dbConnection = new DbConnectionMock();
            dynamic queries = new DbCommandRepository(dbConnection, "TestData");
            string commandText = queries.Select; // Behavior inherited from FileRepository
            TestDataUtility.AreEqual(commandText, ".sql", "select");
        }

        [TestMethod]
        public void GetDbCommand()
        {
            var dbConnection = new DbConnectionMock();
            dynamic queries = new DbCommandRepository(dbConnection, "TestData");
            IDbCommand command = queries.Select();
            TestDataUtility.AreEqual(command.CommandText, ".sql", "select");

            Assert.IsInstanceOfType(command, typeof(DbCommandMock));
            var commandMock = (DbCommandMock)command;
            Assert.AreEqual(0, commandMock.ParameterMocks.Count);
        }

        [TestMethod]
        public void GetDbCommandWithParameters()
        {
            var dbConnection = new DbConnectionMock();
            dynamic queries = new DbCommandRepository(dbConnection, "TestData");
            IDbCommand command = queries.Select("Param1", 123);
            TestDataUtility.AreEqual(command.CommandText, ".sql", "select");

            Assert.IsInstanceOfType(command, typeof(DbCommandMock));
            var commandMock = (DbCommandMock)command;
            Assert.AreEqual(2, commandMock.ParameterMocks.Count);

            Assert.AreEqual("0", commandMock.ParameterMocks[0].ParameterName);
            Assert.AreEqual("Param1", commandMock.ParameterMocks[0].Value);

            Assert.AreEqual("1", commandMock.ParameterMocks[1].ParameterName);
            Assert.AreEqual(123, commandMock.ParameterMocks[1].Value);
        }

        [TestMethod]
        public void GetDbCommandWithNamedParameters()
        {
            var dbConnection = new DbConnectionMock();
            dynamic queries = new DbCommandRepository(dbConnection, "TestData");
            IDbCommand command = queries.Select(new Dictionary<string, object> { ["Name1"] = "Param1", ["Name2"] = 123 });
            TestDataUtility.AreEqual(command.CommandText, ".sql", "select");

            Assert.IsInstanceOfType(command, typeof(DbCommandMock));
            var commandMock = (DbCommandMock)command;
            Assert.AreEqual(2, commandMock.ParameterMocks.Count);

            Assert.AreEqual("Name1", commandMock.ParameterMocks[0].ParameterName);
            Assert.AreEqual("Param1", commandMock.ParameterMocks[0].Value);

            Assert.AreEqual("Name2", commandMock.ParameterMocks[1].ParameterName);
            Assert.AreEqual(123, commandMock.ParameterMocks[1].Value);
        }
    }
}
