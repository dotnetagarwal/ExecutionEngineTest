using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.ResultProcessing
{
    public class RecordLevelControlResultProcessing : BaseResultProcessing, IProcessControlsResult
    {
        private readonly ICompareObjectsService _compareObjectsService;
        private readonly ILogger<RecordLevelControlResultProcessing> _logger;
        private readonly IConfiguration _configuration;
        public RecordLevelControlResultProcessing(ICompareObjectsService compareObjectsService, ILogger<RecordLevelControlResultProcessing> logger, IConfiguration configuration)
        {
            _compareObjectsService = compareObjectsService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> ExecuteBusinessRules(ControlLevelRules controlLevelRules, ControlConfigurationDetail controlConfigurationDetails)
        {
            try
            {
                //Load BusinessRules Assesmbly
                //Findout Rule method in RecordLevelBusinessRules Class
                //If it doesn't exist, throw exception "Rule Not Implemented"
                //Execute the Rule and break the chain if successful
                var greenStatusRules = controlLevelRules.Green.Rules;
                dynamic res = new BusinessRulesResponse();
                foreach (Rules rule in greenStatusRules)
                {
                    string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + _configuration.GetSection("BusinessRuleDll").Value;
                    Assembly assesmbly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                    Type type = assesmbly.ExportedTypes.Where(t => t.FullName == "ESOC.CLTPull.ExecutionEngine.BusinessRules.RecordLevel_Green").FirstOrDefault(); //TODO We need to create this RecordLevel_Green from ControlType and Status Fusion
                    var rulesType = Activator.CreateInstance(type, _compareObjectsService);
                    var myMethodwithParams = type.GetMethod(rule.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    res = myMethodwithParams.Invoke(rulesType, new[] { controlConfigurationDetails });
                    if (res.IsSuccess)
                        break;
                }
                //For RED scenarios also, the same will apply
                return res.Data as String;
            }
            catch(Exception)
            {
                throw;
            }
        }
        public async Task<string> ProcessControlsResult(Resultsetstructure resultsetstructure)
        {
            JObject lhsJObject = JsonConvert.DeserializeObject<JObject>(resultsetstructure.Lhsresult.Result);
            JObject rhsJObject = JsonConvert.DeserializeObject<JObject>(resultsetstructure.Rhsresult.Result);
            var comparisonReport = await _compareObjectsService.CompareTwoResultSets(lhsJObject, rhsJObject, "resultSet");
            _logger.LogInformation($"LHS Only Count: {comparisonReport.LHSOnly.Count}, RHS Only Count: {comparisonReport.RHSOnly.Count}");
            return JsonConvert.SerializeObject(comparisonReport);
        }               
    }
}
