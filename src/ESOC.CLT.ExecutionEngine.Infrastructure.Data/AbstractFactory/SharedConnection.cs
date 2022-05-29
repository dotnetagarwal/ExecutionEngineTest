using ESOC.CLT.ExecutionEngine.Infrastructure.Data.AbstractFactory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data
{
    public class SharedConnection : IDbConnectionFactory
    {
        private readonly IRelationDbFactory _relationDbFactory;
        private readonly ILogger<SharedConnection> _logger;
        public SharedConnection(IRelationDbFactory relationDbFactory, ILogger<SharedConnection> logger)
        {
            _relationDbFactory = relationDbFactory;
            _logger = logger;
        }
        public async  Task<IDbDataAdapter> FetchDataAdaptor<T>(string connectionString, string query)
        {
            try
            {
                var dbConnection = await _relationDbFactory.DbConnectionFactory<T>(connectionString);
                if (dbConnection.State == ConnectionState.Closed)
                    dbConnection.Open();
                var dbCommand = await _relationDbFactory.DbCommandFactory<T>(query, dbConnection);
                var dbAdaptor = await _relationDbFactory.DbAdaptorFactory<T>(dbCommand);
                dbConnection.Close();
                return dbAdaptor;
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Create Database Connection: {ex.StackTrace} ");
                throw;
            }
        }
    }
}
