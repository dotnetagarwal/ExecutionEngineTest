namespace ESOC.CLTPull.ExecutionEngine.Core.Contracts
{
    using Newtonsoft.Json.Linq;
    using ESOC.CLTPull.ExecutionEngine.Core.Entities.Common;
    using System.Threading.Tasks;

    public interface ICompareObjectsService
    {
        Task<ResultSetComparisonReport> CompareTwoResultSets(JObject lhs, JObject rhs, string rootNode);


    }
}