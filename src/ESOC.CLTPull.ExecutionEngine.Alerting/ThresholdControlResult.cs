using ESOC.CLTPull.ExecutionEngine.Alerting.Events;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.Common;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Alerting
{
    public class ThresholdControlResult : IControlResult
    {
        public dynamic ProcessControlStatus(ControlConfigurationDetail controlConfigurationDetails, ResultSetComparisonReport comparisonReport)
        {
            throw new NotImplementedException();
        }
    }
}
