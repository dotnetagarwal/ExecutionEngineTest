using ESOC.CLTPull.ExecutionEngine.Core.Entities.Common;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
using ESOC.CLTPull.ExecutionEngine.Core.Models;
using System;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.ResultProcessing
{
    public class ThresholdControlResultProcessing : BaseResultProcessing, IProcessControlsResult
    {
        public Task<string> ExecuteBusinessRules(ControlLevelRules controlLevelRules, ControlConfigurationDetail controlConfigurationDetail)
        {
            throw new NotImplementedException();
        }

        public Task<string> ProcessControlsResult(Resultsetstructure resultsetstructure)
        {
            throw new NotImplementedException();
        }
    }
}
