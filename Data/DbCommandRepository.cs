using ShUtilities.IO;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;

namespace ShUtilities.Data
{
    /// <summary>
    /// Exposes the contents of *.sql files in a directory as <see cref="IDbCommand"/> objects.
    /// </summary>
    /// <remarks>Inspired by https://github.com/nackjicholson/aiosql </remarks>
    public class DbCommandRepository : FileRepository
    {
        public DbCommandRepository(IDbConnection connection, string directory) : this(connection, directory, "{0}")
        {
        }

        public DbCommandRepository(IDbConnection connection, string directory, string parameterNameTemplate) : base(Path.Combine(directory, "*.sql"))
        {
            Connection = connection;
            ParameterNameTemplate = parameterNameTemplate;
        }

        public IDbConnection Connection { get; set; }
        public string ParameterNameTemplate { get; set; }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] parameters, out object value)
        {
            bool result = TryFileContents(binder.Name, out string commandText);
            if (!result)
            {
                value = null;
            }
            else
            {
                var command = Connection.CreateCommand();
                command.CommandText = commandText;
                if (parameters != null)
                {
                    if (parameters.Length == 1 && parameters[0] is IDictionary<string, object> parametersByName)
                    {
                        foreach ((string name, object parameterValue) in parametersByName)
                        {
                            IDbDataParameter parameter = command.CreateParameter();
                            parameter.ParameterName = name;
                            parameter.Value = parameterValue;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            IDbDataParameter parameter = command.CreateParameter();
                            parameter.ParameterName = string.Format(ParameterNameTemplate, i);
                            parameter.Value = parameters[i];
                        }
                    }
                }
                value = command;
            }
            return result;
        }
    }
}
