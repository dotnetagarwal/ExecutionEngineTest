namespace ESOC.CLTPull.ExecutionEngine.Test.ControlExecutionResultsService
{
    using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
    using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
    using ESOC.CLTPull.ExecutionEngine.Core.EventHandlers;
    using ESOC.CLTPull.ExecutionEngine.Core.Models;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Xunit;

    public class ControlExecutionEventHandlerTest
    {
        private readonly Mock<ILogger<ControlExecutionEventHandler>> _logger;
        private readonly Mock<IControlExecutionResultsService> _controlExecutionResultsService;

        private readonly string dummyJsonFilePath;
        public ControlExecutionEventHandlerTest(WebTestFixture factory)
        {
            _logger = new Mock<ILogger<ControlExecutionEventHandler>>();
            _controlExecutionResultsService = new Mock<IControlExecutionResultsService>();
            dummyJsonFilePath = @"ExecutionEngineAlgo\ExecutionEngineAdapter.json";
        }

        [Fact]
        public void Check_EventHandler_DataParser_To_ControlExecutionService()
        {
            string expectedResult = "DataParsedSuccessfully";
            string actualResult = string.Empty;
            string controlExecutionEventData = System.IO.File.ReadAllText(dummyJsonFilePath);
            ControlExecutionEvent controlExecutionEvent = new ControlExecutionEvent
            {
                Message = controlExecutionEventData
            };

            _controlExecutionResultsService.Setup(x => x.TriggerControlExecution(It.IsAny<ControlExecutionEvent>())).ReturnsAsync(TriggerControlExecution(controlExecutionEvent, ref actualResult));
            var _eventHandler = new ControlExecutionEventHandler(_logger.Object, _controlExecutionResultsService.Object);
            _eventHandler.Handle(controlExecutionEvent).Wait();

            Assert.Equal(actualResult, expectedResult);

        }


        #region Fakedata for EventHandler DataParser to ControlExecutionService

        private IList<ControlExecutionResult> TriggerControlExecution(ControlExecutionEvent controlExecutionEvent, ref string actualResult)
        {
            try
            {
                var controlDetails = JsonConvert.DeserializeObject<ControlConfigurationDetail>(controlExecutionEvent.Message);
                actualResult = "DataParsedSuccessfully";
            }
            catch
            {
                actualResult = "DataParsingFailed";
            }

            return new List<ControlExecutionResult>();
        }
        #endregion
    }
}
