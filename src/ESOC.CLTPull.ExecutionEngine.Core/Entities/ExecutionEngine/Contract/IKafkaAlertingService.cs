
namespace ESOC.CLTPull.ExecutionEngine.Core.Contracts
{
    public interface IKafkaAlertingService
    {
        void PublishControlResult(dynamic resultStatus);
    }
}
