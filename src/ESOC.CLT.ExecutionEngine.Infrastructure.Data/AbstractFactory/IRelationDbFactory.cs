using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data.AbstractFactory
{
    public interface IRelationDbFactory
    {
        Task<IDbConnection> DbConnectionFactory<T>(string connectionString);
        Task<IDbCommand> DbCommandFactory<T>(string query, IDbConnection dbConnection);
        Task<IDbDataAdapter> DbAdaptorFactory<T>(IDbCommand dbCommand);

    }
}
