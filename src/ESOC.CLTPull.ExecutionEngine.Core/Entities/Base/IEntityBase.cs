namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.Base
{
    public interface IEntityBase<TId>
    {
        TId Id { get; }
    }
}
