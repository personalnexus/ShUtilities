using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ShUtilities.Collections;

namespace ShUtilitiesTest.Mocks
{
    internal class DbCommandMock : IDbCommand
    {
        private List<DbParameterMock> _parameters = new List<DbParameterMock>();

        public string CommandText { get; set; }
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }
        public IDbConnection Connection { get; set; }

        public IDataParameterCollection Parameters { get; }

        public IReadOnlyList<DbParameterMock> ParameterMocks => _parameters;

        public IDbTransaction Transaction { get; set; }
        public UpdateRowSource UpdatedRowSource { get; set; }

        public void Cancel()
        {
            throw new System.NotImplementedException();
        }

        public IDbDataParameter CreateParameter() => _parameters.AddNew();

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteNonQuery()
        {
            throw new System.NotImplementedException();
        }

        public IDataReader ExecuteReader()
        {
            throw new System.NotImplementedException();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new System.NotImplementedException();
        }

        public object ExecuteScalar()
        {
            throw new System.NotImplementedException();
        }

        public void Prepare()
        {
            throw new System.NotImplementedException();
        }
    }
}