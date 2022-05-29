using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract
{
    public interface IOracleApplyDataExtractionRules
    {
        Task<string> ApplyDataExtractionRules(Oracle dataAdapter);
    }
}
