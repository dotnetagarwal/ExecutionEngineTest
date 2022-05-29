using ESOC.CLTPull.ExecutionEngine.Alerting.Events;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.Common;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ESOC.CLTPull.ExecutionEngine.Alerting
{
    public class RecordLevelControlResult : IControlResult
    {
        private readonly IOOBAttachment oobAttachment;
        private readonly ILogger<RecordLevelControlResult> _logger;
        public RecordLevelControlResult(IOOBAttachment _oobAttachment, ILogger<RecordLevelControlResult> logger)
        {
            oobAttachment = _oobAttachment;
            _logger = logger;
        }

        public dynamic ProcessControlStatus(ControlConfigurationDetail controlConfigurationDetails, ResultSetComparisonReport comparisonReport)
        {
            List<RecordCountEvent> recordCountEvent = new List<RecordCountEvent>();
            RecordCountEvent countEvent = new RecordCountEvent();
            JObject lhs = JsonConvert.DeserializeObject<JObject>(controlConfigurationDetails.Resultsetstructure.Lhsresult.Result);
            JObject lhsObject = (JObject)lhs["resultSet"];
            if (lhsObject.Count == 0)//Count at LHS is zero
            {
                _logger.LogInformation($"Count at LHS is zero");
                countEvent = CreateStatus(controlConfigurationDetails, comparisonReport, ResultStatus.RED.ToString());
                countEvent.TicketingCaseId = 1;
                recordCountEvent.Add(countEvent);
                return recordCountEvent;
            }
            if (comparisonReport.LHSOnly.Count > 0)//records not found at RHS
            {
                _logger.LogInformation($"Record level Control Counts are not Matching");
                countEvent = CreateStatus(controlConfigurationDetails, comparisonReport, ResultStatus.RED.ToString());
                string attachmentPath = oobAttachment.CreateAttachment(controlConfigurationDetails, comparisonReport);
                countEvent.AttachmentPath = attachmentPath;
                countEvent.TicketingCaseId = 1;//should be part of business rules to decide which ticketing id is needed.
                recordCountEvent.Add(countEvent);
                return recordCountEvent;
            }
            _logger.LogInformation($"Record level Control Result Status is Green");
            countEvent = CreateStatus(controlConfigurationDetails, comparisonReport, ResultStatus.GREEN.ToString());//green case
            recordCountEvent.Add(countEvent);
            return recordCountEvent;
        }

        public RecordCountEvent CreateStatus(ControlConfigurationDetail controlConfigurationDetails, ResultSetComparisonReport comparisonReport, string status)
        {
            JObject lhs = JsonConvert.DeserializeObject<JObject>(controlConfigurationDetails.Resultsetstructure.Lhsresult.Result);
            JObject lhsObject = (JObject)lhs["resultSet"];
            //JObject rhs = JsonConvert.DeserializeObject<JObject>(contract.ControlConfigurationDetail.Resultsetstructure.Lhsresult.Result);
            //JObject rhsObject = (JObject)rhs["resultSet"];
            RecordCountEvent recordCountEvent = new RecordCountEvent()
            {
                MasterID = controlConfigurationDetails.ControlInformation.ControlId,
                ControlExecutionDate = DateTime.Now,
                LookbackDate = DateTime.Now.AddDays(Convert.ToInt32(controlConfigurationDetails.ControlInformation.LookBackDate)),
                SourceCount = lhsObject.Count,
                DestinationCount = lhsObject.Count - comparisonReport.LHSOnly.Count,
                OOBCount = comparisonReport.LHSOnly.Count,
                ExecutionResult = status,
                ControlDescription = controlConfigurationDetails.ControlInformation.ControlDescription,
                RecordOriginatingSource = "ExecutionEngine"
            };
            return recordCountEvent;
        }
    }
}
