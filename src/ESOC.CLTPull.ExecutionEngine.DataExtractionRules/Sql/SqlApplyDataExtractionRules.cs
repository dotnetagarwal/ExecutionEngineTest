using ESOC.CLT.ExecutionEngine.Infrastructure.Data;
using ESOC.CLTPull.ExecutionEngine.Core;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.DataExtractionRules
{
  public class SqlApplyDataExtractionRules : ApplyDataExtractionRules, ISqlApplyDataExtractionRules
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public SqlApplyDataExtractionRules(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public async Task<string> ApplyDataExtractionRules(Sql adapterInfo)
        {
            // Sql sqlDataAdapter = CastAdapterInformationToRespectiveDataAdapter<Sql>(adapterInfo);

            //Add Sql Server code 
            string connectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", adapterInfo.ServerName, adapterInfo.DatabaseName, adapterInfo.UserName, adapterInfo.Password);
            IDataAdapter dataAdaptor = _dbConnectionFactory.FetchDataAdaptor<Sql>(connectionString, adapterInfo.DataFetchRules.Query.Text).Result;
            DataSet dataSet = new DataSet();
            dataAdaptor.Fill(dataSet);
            int totalCount = 0;
            if (dataSet.Tables.Count > 0)
                totalCount = dataSet.Tables[0].Rows.Count;
            return await Task.Run(() => JsonConvert.SerializeObject(new { resultSet = new { Count = totalCount } }));
        }
    }
}
