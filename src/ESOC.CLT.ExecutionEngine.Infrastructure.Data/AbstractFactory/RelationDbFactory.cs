using ESOC.CLT.ExecutionEngine.Infrastructure.Data.AbstractFactory;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data
{
    public class RelationDbFactory:IRelationDbFactory
    {
        public async Task<IDbConnection> DbConnectionFactory<T>(string connectionString)
        {
            Type parameterType = typeof(T);
            switch (parameterType.Name)
            {
                case nameof(Sql):
                    return new SqlConnection(connectionString);
                case nameof(ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Oracle):
                    return  new OracleConnection(connectionString);
                default:
                    throw new ArgumentException("Type of Db Connection is invalid");
            }

        }
        public async Task<IDbCommand> DbCommandFactory<T>(string query, IDbConnection dbConnection)
        {
            Type parameterType = typeof(T);
            switch (parameterType.Name)
            {
                case nameof(Sql):
                    return new SqlCommand(query, (SqlConnection)dbConnection);
                case nameof(ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Oracle):
                    return new OracleCommand(query, (OracleConnection)dbConnection);
                default:
                    throw new ArgumentException("Type of Db Command is invalid");
            }

        }
        public async Task<IDbDataAdapter> DbAdaptorFactory<T>(IDbCommand dbCommand)
        {
            Type parameterType = typeof(T);
            switch (parameterType.Name)
            {
                case nameof(Sql):
                    return new SqlDataAdapter((SqlCommand)dbCommand);
                case nameof(ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Oracle):
                    return new OracleDataAdapter((OracleCommand)dbCommand);
                default:
                    throw new ArgumentException("Type of Db Adaptor is invalid");
            }

        }
    }
}
