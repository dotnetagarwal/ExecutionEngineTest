using ESOC.CLTPull.ExecutionEngine.Core.Entities.Common;
using ESOC.CLTPull.ExecutionEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract
{
   public interface IProcessControlsResult
    {
        Task<string> ProcessControlsResult(Resultsetstructure resultsetstructure);
        Task<string> ExecuteBusinessRules(ControlLevelRules controlLevelRules, ControlConfigurationDetail controlConfigurationDetail);
    }
}
