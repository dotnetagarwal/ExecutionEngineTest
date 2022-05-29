using Xunit;
using Newtonsoft.Json;
using System.Data;
using System;
using ESOC.CLTPull.ExecutionEngine.Test.JsonCrudPoc;
using Newtonsoft.Json.Linq;
using ESOC.CLT.ExecutionEngine.Infrastructure.Data;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using ESOC.CLTPull.ExecutionEngine.Core;

namespace ESOC.CLTPull.ExecutionEngine.Test
{
   public class FactoryUnitTest
    {
        [Fact]
        public void InsertJsonDatainDB()
        {
            //IScheduleStructure scheduleStructure = new Daily()
            //{
            //    StartTime = DateTime.Now,
            //    EndTime = DateTime.Now.AddDays(2),
            //    ReccurenceInterval = 2
            //};



            //ControlScheduleConfiguration controlScheduleConfiguration = new ControlScheduleConfiguration(scheduleStructure)
            //{
            //    ScheduleConfigurationId = "1",
            //    ScheduleFrequency = Frequency.Daily
            //};
            //Assert.NotNull(controlScheduleConfiguration);
            //string result = controlScheduleConfiguration.ScheduleData;
            //string connectionString = "Server=DBSED4108;database=CLTPULL;Trusted_Connection=true;";
            //string query = "usp_insertControlScheduleConfiguration";
            //var dbConnection = RelationDbFactory.DbConnectionFactory<Sql>(connectionString).Result;
            ////IDbConnection dbConnection = dbConnectionFactory.CreateConnection(connectionString);

            //IDbCommand dbCommand = RelationDbFactory.DbCommandFactory<Sql>(query, dbConnection).Result;
            //dbCommand.CommandType = CommandType.StoredProcedure;
            //SqlServerDbConnection.AddParameter(dbCommand, "@MasterId", "xyz");
            //SqlServerDbConnection.AddParameter(dbCommand, "@ScheduleConfigurationDetail", result);
            //dbConnection.Open();
            //dbCommand.ExecuteNonQuery();
            //dbConnection.Close();
        }
        [Fact]
        public void ReadJsonDataFromDb()
        {
            //string connectionString = "Server=DBSED4108;database=CLTPULL;Trusted_Connection=true;";
            //string query = "usp_GetDataFromControlScheduleConfiguration";
            //string jsonOutputParam = string.Empty;
            //var dbConnection = RelationDbFactory.DbConnectionFactory<Sql>(connectionString).Result;
            ////IDbConnection dbConnection = dbConnectionFactory.CreateConnection(connectionString);
            //IDbCommand dbCommand = RelationDbFactory.DbCommandFactory<Sql>(query, dbConnection).Result;
            //dbCommand.CommandType = CommandType.StoredProcedure;
            ////SqlServerDbConnection.CreateOutputParameter(dbCommand, "@jsonData", DbType.String, 1000);
            //IDbDataAdapter dataAdaptor = RelationDbFactory.DbAdaptorFactory<Sql>(dbCommand).Result;
            //DataSet dataSet = new DataSet();
            //dataAdaptor.Fill(dataSet);
            //if (dataSet.Tables.Count > 0)
            //    jsonOutputParam = Convert.ToString(dataSet.Tables[0].Rows[0]["ScheduleConfigurationDetail"]);
            //var myDetails = JsonConvert.DeserializeObject<JObject>(jsonOutputParam);
        }

        //////[Fact]
        //////public void FrequencyHourlyTest()
        //////{
        //////    IScheduleStructure scheduleStructure = new Daily()
        //////    {
        //////        StartTime = DateTime.Now,
        //////        EndTime = DateTime.Now.AddDays(2),
        //////        ReccurenceInterval = 2
        //////    };

        //////    ControlScheduleConfiguration controlScheduleConfiguration = new ControlScheduleConfiguration(scheduleStructure)
        //////    {
        //////        ScheduleConfigurationId = "1",
        //////        ScheduleFrequency = Frequency.Daily
        //////    };
        //////    Assert.NotNull(controlScheduleConfiguration);
        //////    string data = controlScheduleConfiguration.ScheduleData;
        //////    JObject keyValuePairs = controlScheduleConfiguration.DeserializeConfigurationData(data);
        //////}

    }
}
