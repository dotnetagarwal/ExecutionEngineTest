using ESOC.CLT.ExecutionEngine.Infrastructure.Data;
using ESOC.CLT.ExecutionEngine.Infrastructure.Data.AbstractFactory;
using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
using ESOC.CLTPull.ExecutionEngine.Core.Models;
using ESOC.CLTPull.ExecutionEngine.DataExtractionRules;
using ESOC.CLTPull.ExecutionEngine.Test.TestData;
using ESOC.CLTPull.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ESOC.CLTPull.ExecutionEngine.Test
{
    public class ControlExecutionResultsServiceTest : IClassFixture<WebTestFixture>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly IControlExecutionResultsService _controlExecutionResultsService;
        private readonly ILogger<SharedConnection> logger;
        private readonly ILogger<NASConnection> naslogger;
        private readonly IBusinessRulesService _businessRulesService;

        public ControlExecutionResultsServiceTest(WebTestFixture factory)
        {
            this._factory = factory;
            _controlExecutionResultsService = factory.Services.GetRequiredService<IControlExecutionResultsService>();
            _businessRulesService = factory.Services.GetRequiredService<IBusinessRulesService>();
        }

        [Fact]
        public void Test1()
        {
            string fileName = @"ExecutionEngineAlgo\ExecutionEngineAdapter.json";
            string jsonContent = System.IO.File.ReadAllText(fileName);

            string fakeEventData = string.Empty;
            string currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, @"ExecutionEngineAlgo\ExecutionEngineAdapter.json");
            fakeEventData = System.IO.File.ReadAllText(filePath);

            ControlExecutionEvent controlExecutionEvent = new ControlExecutionEvent();
            //controlExecutionEvent.Message = jsonContent;
            ExecutionEngineContract executionEngineContract = new ExecutionEngineContract();
            executionEngineContract.ControlScheduleInstanceId = "1";
            executionEngineContract.ControlConfigurationDetail = jsonContent;
            controlExecutionEvent.Message = JsonConvert.SerializeObject(executionEngineContract);
            var actionResult = _controlExecutionResultsService.TriggerControlExecution(controlExecutionEvent);
            //Assert
            // var viewResult = Assert.IsType<ViewResult>(actionResult);
        }

        [Fact]
        public void GetBusinessRulesTest() {

            var businessRuleList = _businessRulesService.GetBusinessRules("ThresholdLevel");
            //Assert.
         }

        [Fact]
        public void ApplyDataExtractionRulesForNASTest()
        {
            Mock<NAS> nasAdapter = new Mock<NAS>();
            Mock<INASConnection> mockNASConnection = new Mock<INASConnection>();
            Mock<ILogger<INASApplyDataExtractionRules>> mockLogger = new Mock<ILogger<INASApplyDataExtractionRules>>();
            mockNASConnection.Setup(x => x.GetFileData(nasAdapter.Object)).Returns(Task.Run(() => new string[5]));
            NASApplyDataExtractionRules tempNASApplyDataExtractionRules = new NASApplyDataExtractionRules(mockNASConnection.Object, mockLogger.Object);
            var result=tempNASApplyDataExtractionRules.ApplyDataExtractionRules(nasAdapter.Object);
            Assert.IsType<string>(result.Result);
        }

        [Fact]
        public void GetFilesCountTest()
        {
            NASConnection nasConnection = new NASConnection(naslogger);
            var paths=CreateTestJson();
            var adapterinfo = NASTestData.GetNASTestData(paths);
            var result=nasConnection.GetFileData(adapterinfo).Result;
            Assert.Equal(new List<string>(), result);
            DeleteTestJson(paths);
        }

        private string[] CreateTestJson()
        {
            string fileName = "TestFile.txt";
            var directory = Directory.GetCurrentDirectory();
            var parent = Directory.GetParent(directory).Parent.Parent.FullName;
            string folderName = @"TestJson";
            string folderPath = Path.Combine(parent, folderName);
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);
            using (FileStream fs = File.Create(filePath)) { }
                string[] paths = {
                folderPath
            };

            return paths;
        }
        private void DeleteTestJson(string[] paths)
        {
            foreach (var path in paths)
            {
                Directory.Delete(path, true);
            }
            
        }

        [Fact]
        public void ApplyDataExtractionRulesForSqlTest()
        {
            Sql sqlAdapter = new Sql() {
                DataFetchRules=new SqlDataFetchRules() { 
                    Query=new Core.Entities.ExecutionEngine.Query() {
                    Text="test"}
                }
            };
            Mock<IDbDataAdapter> dbDataAdapter = new Mock<IDbDataAdapter>()
            {
                DefaultValue = DefaultValue.Mock,
            };
            Mock<IDbConnectionFactory> mockSQLConnection = new Mock<IDbConnectionFactory>();
            mockSQLConnection.Setup(x => x.FetchDataAdaptor<Sql>(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.Run(() => dbDataAdapter.Object));
            SqlApplyDataExtractionRules tempNASApplyDataExtractionRules = new SqlApplyDataExtractionRules(mockSQLConnection.Object);
            var result = tempNASApplyDataExtractionRules.ApplyDataExtractionRules(sqlAdapter);
            Assert.IsType<string>(result.Result);
        }

        [Fact]
        public void FetchDataAdaptorTest()
        {
            Mock<IDbConnection> dbConnection = new Mock<IDbConnection>() { 
                DefaultValue=DefaultValue.Mock            
            };

            Mock<IDbCommand> dbCommand = new Mock<IDbCommand>();
            Mock<IDbDataAdapter> dbDataAdapter = new Mock<IDbDataAdapter>();

            Mock<IRelationDbFactory> mockrelationDbFactory = new Mock<IRelationDbFactory>();
            mockrelationDbFactory.Setup(x => x.DbConnectionFactory<Sql>(It.IsAny<string>())).Returns(Task.Run(() => dbConnection.Object));
            mockrelationDbFactory.Setup(x => x.DbCommandFactory<Sql>(It.IsAny<string>(), dbConnection.Object)).Returns(Task.Run(() => dbCommand.Object));
            mockrelationDbFactory.Setup(x => x.DbAdaptorFactory<Sql>(dbCommand.Object)).Returns(Task.Run(() => dbDataAdapter.Object));

            SharedConnection sharedConnection = new SharedConnection(mockrelationDbFactory.Object, logger);
            var result= sharedConnection.FetchDataAdaptor<Sql>(It.IsAny<string>(), It.IsAny<string>()).Result;
            Assert.NotNull(result);
        }
    }
}
