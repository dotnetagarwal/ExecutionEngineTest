
using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ESOC.CLTPull.ExecutionEngine.BusinessRules
{
    public class RecordLevel_Green 
    {
        private readonly ICompareObjectsService _compareObjectsService;


        public RecordLevel_Green(ICompareObjectsService compareObjectsService)
        {
            _compareObjectsService = compareObjectsService;
        }
        public BusinessRulesResponse TotalCountMatched(ControlConfigurationDetail controlConfigurationDetail)
        {
            
            JObject lhsJObject = JsonConvert.DeserializeObject<JObject>(controlConfigurationDetail.Resultsetstructure.Lhsresult.Result);
            JObject rhsJObject = JsonConvert.DeserializeObject<JObject>(controlConfigurationDetail.Resultsetstructure.Rhsresult.Result);
            var comparisonReport = _compareObjectsService.CompareTwoResultSets(lhsJObject, rhsJObject, "resultSet");
            //_logger.LogInformation($"LHS Only Count: {comparisonReport.LHSOnly.Count}, RHS Only Count: {comparisonReport.RHSOnly.Count}");
            BusinessRulesResponse response = new BusinessRulesResponse();
            //TODO pass comparison report
            response.Data = JsonConvert.SerializeObject(comparisonReport);
            response.IsSuccess = true;
            //Can call other services and write actual code here
            return response;
        }

        public bool IdByIdComparison(ControlConfigurationDetail controlConfigurationDetail)
        {
            //Can call other services and write actual code here
            return true;
        }
    }
}
