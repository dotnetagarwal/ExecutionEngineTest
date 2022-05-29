using ESOC.CLTPull.ExecutionEngine.API.Controllers;
using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
using ESOC.CLTPull.ExecutionEngine.Test.JsonCrudPoc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xunit;

namespace ESOC.CLTPull.ExecutionEngine.Test
{
    public class UnitTest1
    {
        private readonly Mock<IControlExecutionResultsService> _controlExecutionResultsServiceMock;
        private readonly Mock<ILogger<ControlExecutionResultsController>> _loggerMock;
        public UnitTest1()
        {
            _controlExecutionResultsServiceMock = new Mock<IControlExecutionResultsService>();
            _loggerMock = new Mock<ILogger<ControlExecutionResultsController>>();
        }

        [Fact]
        public void Test1()
        {
            string fakeEventData = string.Empty;
            string currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, "json1.json");
            fakeEventData = System.IO.File.ReadAllText(filePath);
            var controlExecutionResultsController = new ControlExecutionResultsController(_loggerMock.Object, _controlExecutionResultsServiceMock.Object);

            var actionResult = controlExecutionResultsController.TriggerControlExecution(fakeEventData);
            //Assert
            // var viewResult = Assert.IsType<ViewResult>(actionResult);
        }

        [Fact]
        public void FrequencyHourlyTest()
        {
            IScheduleStructure scheduleStructure = new Daily()
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(2),
                ReccurenceInterval = 2
            };



            ControlScheduleConfiguration controlScheduleConfiguration = new ControlScheduleConfiguration(scheduleStructure)
            {
                ScheduleConfigurationId = "1",
                ScheduleFrequency = Frequency.Daily
            };
            Assert.NotNull(controlScheduleConfiguration);
            string data = controlScheduleConfiguration.ScheduleData;
            var keyValuePairs = controlScheduleConfiguration.DeserializeConfigurationData(data);
        }
    }
}


