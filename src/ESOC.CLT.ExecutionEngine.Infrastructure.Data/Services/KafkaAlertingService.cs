using Confluent.Kafka;
using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
using ESOC.CLTPull.ExecutionEngine.Core.Events;
using ESOC.CLTPull.ExecutionEngine.Core.Extensions;
using EventBus;
using Microsoft.Extensions.Logging;
using System;

namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data.Services
{
    public class KafkaAlertingService: IKafkaAlertingService
    {
        private readonly IEventBus _eventBus;
        private readonly ConsumerConfig _config;
        private readonly ILogger<KafkaAlertingService> _logger;

        public KafkaAlertingService(IEventBus eventBus, ConsumerConfig config, ILogger<KafkaAlertingService> logger)
        {
            _eventBus = eventBus;
            _config = config;
            _logger = logger;
        }
        public void PublishControlResult(dynamic resultStatus)
        {
            try
            {
                var eventMessage = JsonExtensions.ToJson(resultStatus);
                PublishServiceNowInputIntegrationEvent(eventMessage);
                //_logger.LogInformation("PublishScheduleAlerts: published to CurrentScheduleEvent");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }
        void PublishServiceNowInputIntegrationEvent(string controlResult)
        {
            try
            {
                var @event = new GenericEvent
                {
                    Message = controlResult
                };
                _logger.LogInformation($"KafkaAlertingService: event: {@event.Id}, Control Result: {controlResult}");
                _eventBus.PublishAsync("RecordCountDataReceivedEvent", @event);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"KafkaAlertingService: {ex.StackTrace} ");
                _logger.LogCritical($"KafkaAlertingService: {ex.Message} ");
            }
        }
    }
}
