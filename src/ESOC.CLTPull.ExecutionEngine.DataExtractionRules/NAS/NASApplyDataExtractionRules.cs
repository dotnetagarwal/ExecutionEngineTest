using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.DataExtractionRules
{
  public class NASApplyDataExtractionRules : ApplyDataExtractionRules, INASApplyDataExtractionRules
    {
        private readonly INASConnection _nasConnection;
        private readonly ILogger<INASApplyDataExtractionRules> _logger;
        public NASApplyDataExtractionRules(INASConnection nasConnection, ILogger<INASApplyDataExtractionRules> logger)
        {
            _nasConnection = nasConnection;
            _logger = logger;
        }
        public async Task<string> ApplyDataExtractionRules(NAS adapterInfo)
        {
            try
            {
                var resultData = await _nasConnection.GetFileData(adapterInfo);
                _logger.LogInformation($"NAS result data length: {resultData.Length}");
                JObject results = new JObject();
                for (int i = 0; i < resultData.Length; i++)
                {
                    var data = new JProperty(resultData[i], resultData[i]);
                    if (!results.ContainsKey(data.Name))
                    {
                        results.Add(data);
                    }
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
    }
}
