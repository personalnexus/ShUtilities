using System.Data;
using System.Data.Common;

namespace ShUtilitiesTest.Mocks
{
    internal class DbParameterMock : IDbDataParameter
    {
        public DbParameterMock()
        {
        }

        public byte Precision { get; set; }
        public byte Scale { get; set; }
        public int Size { get; set; }
        public DbType DbType { get; set; }
        public ParameterDirection Direction { get; set; }

        public bool IsNullable { get; }

        public string ParameterName { get; set; }
        public string SourceColumn { get; set; }
        public DataRowVersion SourceVersion { get; set; }
        public object Value { get; set; }
    }
}