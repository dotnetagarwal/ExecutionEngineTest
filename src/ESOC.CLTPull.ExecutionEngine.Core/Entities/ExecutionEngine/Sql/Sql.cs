using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine
{
    public class Sql : IDataExtractionRules
    {
       private ISqlApplyDataExtractionRules _sqlApplyDataExtractionRules;

        ////////public Sql(ISqlApplyDataExtractionRules sqlApplyDataExtractionRules)
        ////////{
        ////////    _sqlApplyDataExtractionRules = sqlApplyDataExtractionRules;
        ////////}

        ////public Sql()
        ////{
        ////    _sqlApplyDataExtractionRules = new TempSqlApplyDataExtractionRules();
        ////}

        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string port { get; set; }
        public string Password { get; set; }
        public SqlDataFetchRules DataFetchRules { get; set; }

        public async Task<string> AcceptDataExtractionRules(dynamic adapterHandler)
        {
             return await Task.Run(() => adapterHandler.ApplyDataExtractionRules(this));
          //  return _sqlApplyDataExtractionRules.ApplyDataExtractionRules(this);
            

            
            //return dataExtractionRulesVisitor.VisitSql(this);
        }
    }

    public class SqlDataFetchRules
    {
        public string LookbackDate { get; set; }
        public Query Query { get; set; }
    }

    public class Query
    {
        public Params Params { get; set; }
        public string Text { get; set; }
    }

    public class Params
    {
        public string[] Param { get; set; }
    }
}
