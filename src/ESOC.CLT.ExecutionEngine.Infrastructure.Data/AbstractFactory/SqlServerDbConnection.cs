using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data
{
    public class SqlServerDbConnection
    {       
        public static void AddParameter(IDbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
        public static void CreateOutputParameter(IDbCommand command, string name, object type, int size)
        {
            IDbDataParameter outputParameter = command.CreateParameter();
            outputParameter.ParameterName = name;
            outputParameter.Direction = ParameterDirection.Output;
            outputParameter.DbType = (DbType)type;
            outputParameter.Size = size;
            command.Parameters.Add(outputParameter);           
        }

    }
}
