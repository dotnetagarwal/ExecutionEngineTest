using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine
{
    public class Oracle : IDataExtractionRules
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string port { get; set; }
        public string Password { get; set; }
        
        public OracleDataFetchRules DataFetchRules { get; set; }

        public async Task<string> AcceptDataExtractionRules(dynamic adapterHandler)
        {
            try{
                return await adapterHandler.ApplyDataExtractionRules(this);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           
        }
    }

    public class OracleDataFetchRules
    {
        public string LookbackDate { get; set; }
        public OracleQuery Query { get; set; }
    }

    public class OracleQuery
    {
        public OracleParams Params { get; set; }
        public string Text { get; set; }
    }

    public class OracleParams
    {
        public string[] Param { get; set; }
    }
}
