using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Core.Contracts
{
    public interface ISqlHelper
    {

        public string connectionstring { get; set; }
        public  IDbConnection CreateConnection();
        public  IDbCommand CreateCommand();
        public  IDbConnection CreateOpenConnection();
        public  IDbCommand CreateCommand(string commandText, IDbConnection connection);
        public  IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection);
        public  IDataParameter CreateParameter(string parameterName, object parameterValue);



    }
}
