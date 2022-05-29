namespace ESOC.CLTPull.ExecutionEngine.Core.Contracts
{
    using ESOC.CLTPull.ExecutionEngine.Core.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IControlExecutionResultsService
    {
        public Task<IList<ControlExecutionResult>> TriggerControlExecution(ControlExecutionEvent controlExecutionEvent);
        
    }
}