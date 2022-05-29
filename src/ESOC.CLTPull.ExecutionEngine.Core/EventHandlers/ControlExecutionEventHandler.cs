namespace ESOC.CLTPull.ExecutionEngine.Core.EventHandlers
{
    using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
    using ESOC.CLTPull.ExecutionEngine.Core.Models;
    using EventBus;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;
    using System;
    using System.Threading.Tasks;

    public class ControlExecutionEventHandler : IIntegrationEventHandler<ControlExecutionEvent>
    {
        private readonly ILogger<ControlExecutionEventHandler> _logger;
        private readonly IControlExecutionResultsService _controlExecutionResultsService;

        public ControlExecutionEventHandler(ILogger<ControlExecutionEventHandler> logger, IControlExecutionResultsService controlExecutionResultsService)
        {
            _logger = logger;
            _controlExecutionResultsService = controlExecutionResultsService;
        }
        public async Task Handle(ControlExecutionEvent @event)
        {
            try
            {
                using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}- {nameof(ControlExecutionEventHandler)}"))
                {
                    _logger.LogInformation($"ControlExecutionEventHandler: Handling integration event: {@event.Id} at { nameof(ControlExecutionEventHandler)} - ({@event})");

                    var controlExecutionResults = await _controlExecutionResultsService.TriggerControlExecution(@event);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ControlExecutionEventHandler: {ex.StackTrace} ");
                _logger.LogCritical($"ControlExecutionEventHandler: {ex.Message} ");
            }
        }
    }
}
