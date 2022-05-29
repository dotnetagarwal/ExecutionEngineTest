
namespace ESOC.CLTPull.ExecutionEngine.Core.Contracts
{
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        /// <summary>
        ///     Applies all database changes.
        /// </summary>
        /// <returns>Number of affected rows.</returns>
        Task<int> Save();
    }
}
