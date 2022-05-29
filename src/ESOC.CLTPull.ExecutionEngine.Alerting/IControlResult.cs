using ESOC.CLTPull.ExecutionEngine.Core.Entities.Common;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;

namespace ESOC.CLTPull.ExecutionEngine.Alerting
{
    public interface IControlResult
    {
        dynamic ProcessControlStatus(ControlConfigurationDetail details, ResultSetComparisonReport comparisonReport);
    }
}
