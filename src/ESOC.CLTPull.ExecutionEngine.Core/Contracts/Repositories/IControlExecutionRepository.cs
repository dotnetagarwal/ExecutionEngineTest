namespace ESOC.CLTPull.ExecutionEngine.Core.Contracts
{
    using System.Threading.Tasks;

    public interface IControlExecutionRepository
    {
        Task<int> SaveControlExecutionResults(string controlid);
    }
}
