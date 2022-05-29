using ESOC.CLTPull.ExecutionEngine.Core.Entities.Common;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;

namespace ESOC.CLTPull.ExecutionEngine.Alerting
{
    public interface IOOBAttachment
    {
        string CreateAttachment(ControlConfigurationDetail controlConfigurationDetails, ResultSetComparisonReport comparisonReport);
    }
}
