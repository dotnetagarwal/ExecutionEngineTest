using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        Task<IDbDataAdapter> FetchDataAdaptor<T>(string connectionString, string query);
    }
}
