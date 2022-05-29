namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data
{
    using ESOC.CLTPull.ExecutionEngine.Alerting;
    using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
    using ESOC.CLTPull.ExecutionEngine.Core.Entities.Common;
    using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
    using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
    using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Enums;
    using ESOC.CLTPull.ExecutionEngine.Core.Models;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class ControlExecutionResultsService : IControlExecutionResultsService
    {

        private readonly ILogger<ControlExecutionResultsService> _logger;
        private readonly IDataExtractionRulesVisitor _dataExtractionRulesVisitor;
        private readonly Func<String, Object> _adapterHandler;
        private readonly Func<ControlTypes, IProcessControlsResult> _controlResultProcessor;
        private readonly Func<ControlTypes, IControlResult> _controlResult;
        private readonly IKafkaAlertingService _kafkaAlertingService;



        public ControlExecutionResultsService(IDataExtractionRulesVisitor dataExtractionRulesVisitor,
            ILogger<ControlExecutionResultsService> logger, Func<String, dynamic> adapterHandler, Func<ControlTypes, IProcessControlsResult> controlResultProcessor, Func<ControlTypes, IControlResult> controlResult, ICompareObjectsService compareObjectsService,
            IKafkaAlertingService kafkaAlertingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataExtractionRulesVisitor = dataExtractionRulesVisitor;
            _adapterHandler = adapterHandler;
            _controlResultProcessor = controlResultProcessor;
            _controlResult = controlResult;
            _kafkaAlertingService = kafkaAlertingService;
        }

        //Main method to have ExecutionEngine Algorithms
        public async Task<IList<ControlExecutionResult>> TriggerControlExecution(ControlExecutionEvent controlExecutionEvent)
        {
            IList<ControlExecutionResult> controlExecutionResults = new List<ControlExecutionResult>();
            try
            {
                var executionDetails = JsonConvert.DeserializeObject<ExecutionEngineContract>(controlExecutionEvent.Message);
                var controlConfigurationDetails = JsonConvert.DeserializeObject<ControlConfigurationDetail>(executionDetails.ControlConfigurationDetail);
                _logger.LogInformation($"Deserialize object in Execution Engine Contract");
                var comparisonReport = await ProcessDataAdapters(controlConfigurationDetails);
                var controlResultProcessor = _controlResult(Enum.Parse<ControlTypes>(controlConfigurationDetails.ControlInformation.ControlType));
                var status = controlResultProcessor.ProcessControlStatus(controlConfigurationDetails, JsonConvert.DeserializeObject<ResultSetComparisonReport>(comparisonReport));
                _kafkaAlertingService.PublishControlResult(status);

                //TODO We need to pass this Report to Alerting Module


            }
            catch (Exception ex)
            {
               _logger.LogInformation($"Exception in Deserialize object in Execution Engine Contract"+ex.Message);
            }
            return controlExecutionResults;
        }


        public async Task<string> ProcessDataAdapters(ControlConfigurationDetail controlConfigurationDetails)
        {
            //On the basis of ControlType, we will select this step of algorithm
            controlConfigurationDetails.Resultsetstructure = new Resultsetstructure();
            controlConfigurationDetails.Resultsetstructure.Lhsresult = new Lhsresult();
            controlConfigurationDetails.Resultsetstructure.Rhsresult = new Rhsresult();
            controlConfigurationDetails.Resultsetstructure.Lhsresult.Result = await GetLhsResultSet(controlConfigurationDetails);
            controlConfigurationDetails.Resultsetstructure.Rhsresult.Result = await GetRhsResultSet(controlConfigurationDetails);

            Dictionary<string, object> businessRules = GetPatternTypes(controlConfigurationDetails.BusinessRules);
            var businessOperations = businessRules[controlConfigurationDetails.ControlInformation.ControlType];

            var resultProcessor = _controlResultProcessor(Enum.Parse<ControlTypes>(controlConfigurationDetails.ControlInformation.ControlType));
            //TODO HERE we need to pass business rules as a Dictionary in ProcessControlsResult

            var result = await resultProcessor.ExecuteBusinessRules((ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.ControlLevelRules)businessOperations, controlConfigurationDetails);
            // var result =await resultProcessor.ProcessControlsResult(contract.ControlExecutionEvent.Resultsetstructure);

            return result;
        }


        private async Task<string> GetRhsResultSet(ControlConfigurationDetail controlConfigurationDetails)
        {
            var rhsDataAdapters = controlConfigurationDetails.RHS.AdapterInformation.AdapterType;
            //IDataExtractionRulesVisitor visitor = new DataExtractionRulesVisitor();
            _logger.LogInformation($"RHS Adapter Type: { nameof(rhsDataAdapters)}");
            Dictionary<string, object> dataAdapterInfo = GetPropertyDictionaryFromType(rhsDataAdapters);
            string rhsResultSet = null;
            foreach (var dataAdapter in dataAdapterInfo)
            {
                //TODO We need to concat json string for all result type
                //  rhsResultSet = ((IDataExtractionRules)dataAdapter.Value).AcceptDataExtractionRules(_dataExtractionRulesVisitor);

                //I need to pass these dependencies directly into AcceptDataExtractionRules() method
                rhsResultSet = await ((IDataExtractionRules)dataAdapter.Value).AcceptDataExtractionRules(_adapterHandler(dataAdapter.Key));
            }

            return rhsResultSet;
        }


        private async Task<string> GetLhsResultSet(ControlConfigurationDetail controlConfigurationDetails)
        {
            string lhsResultSet = null;
            var lhsDataAdapters = controlConfigurationDetails.LHS.AdapterInformation.AdapterType;
            //IDataExtractionRulesVisitor visitor = new DataExtractionRulesVisitor();
            _logger.LogInformation($"LHS Adapter Type: { nameof(lhsDataAdapters)}");
            Dictionary<string, object> dataAdapterInfo = GetPropertyDictionaryFromType(lhsDataAdapters);

            foreach (var dataAdapter in dataAdapterInfo)
            {
                //TODO We need to concat json string for all result type
                lhsResultSet = await ((IDataExtractionRules)dataAdapter.Value).AcceptDataExtractionRules(_adapterHandler(dataAdapter.Key));
            }

            return lhsResultSet;
        }


        //TODO We need to move the same in some helper class
        public static Dictionary<string, object> GetPropertyDictionaryFromType(object atype)
        {
            if (atype == null) return new Dictionary<string, object>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                if (prp.PropertyType.GetInterfaces().Contains(typeof(IDataExtractionRules))) //TODO We can see if we pass IDataExtractionRules as param
                {
                    object value = prp.GetValue(atype, new object[] { });
                    if (null != value)
                        dict.Add(prp.Name, value);
                }
            }
            return dict;
        }

        public static Dictionary<string, object> GetPatternTypes(object atype)
        {
            if (atype == null) return new Dictionary<string, object>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }
    }
}