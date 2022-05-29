using ESOC.CLT.ExecutionEngine.Infrastructure.Data;
using ESOC.CLTPull.ExecutionEngine.Core.Constants;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.DataExtractionRules.OracleRules
{
    public class OracleApplyDataExtractionRules : ApplyDataExtractionRules, IOracleApplyDataExtractionRules
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<IOracleApplyDataExtractionRules> _logger;

        public OracleApplyDataExtractionRules(IDbConnectionFactory dbConnectionFactory, ILogger<IOracleApplyDataExtractionRules> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }
        public async Task<string> ApplyDataExtractionRules(Core.Entities.ExecutionEngine.Oracle adapterInfo)
        {
            try
            {
                List<string> resultData = new List<string>();
                string connectionString = DataUtility.GetOracleConnectionString(adapterInfo);
                _logger.LogInformation($"Oracle connection string: {connectionString}");
                var query = ReplaceParametersInQuery(adapterInfo);
                _logger.LogInformation($"Oracle query: {query}");
                DataSet dataSet = await GetDataSet(connectionString, query);
                _logger.LogInformation($"DataSet Result Count: {dataSet.Tables.Count}");
                if (dataSet.Tables.Count > 0)
                {
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        resultData.Add(dataSet.Tables[0].Rows[i][0].ToString());
                    }
                }

                JObject results = new JObject();
                for (int i = 0; i < resultData.Count; i++)
                {
                    var data = new JProperty(resultData[i], resultData[i]);
                    results.Add(data);
                }
                return JsonConvert.SerializeObject(new { resultSet = results });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.StackTrace);
                _logger.LogError(exception.Message);
                throw;
            }
        }

        private async Task<DataSet> GetDataSet(string connectionString, string query)
        {
            IDataAdapter dataAdaptor = await _dbConnectionFactory.FetchDataAdaptor<Core.Entities.ExecutionEngine.Oracle>(connectionString, query);
            DataSet dataSet = new DataSet();
            dataAdaptor.Fill(dataSet);
            return dataSet;
        }

        private static string ReplaceParametersInQuery(Core.Entities.ExecutionEngine.Oracle adapterInfo)
        {
            var query = adapterInfo.DataFetchRules.Query.Text;
            if (adapterInfo.DataFetchRules.Query.Params.Param.Any())
            {
                var parameter = adapterInfo.DataFetchRules.Query.Params.Param[0];
                var date = Int32.Parse(adapterInfo.DataFetchRules.LookbackDate);
                query = adapterInfo.DataFetchRules.Query.Text.Replace(parameter, DateTime.Now.AddDays(date).ToString(DateFormats.DateFormatddMMMyyyy));
            }
            return query;
        }
    }
}
