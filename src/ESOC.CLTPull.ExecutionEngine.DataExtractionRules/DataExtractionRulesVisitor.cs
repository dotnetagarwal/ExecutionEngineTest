using System;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;

//WE ARE NOT USING THIS FILE ANY MORE
namespace ESOC.CLTPull.ExecutionEngine.DataExtractionRules
{
    //https://medium.com/geekculture/design-pattern-visitor-pattern-the-most-complicated-design-pattern-with-c-code-sample-f88b608ffb4a //Better One
    //https://exceptionnotfound.net/visitor-pattern-in-csharp/  //Another Perspective
    public class DataExtractionRulesVisitor : IDataExtractionRulesVisitor
    {
      
        public string VisitNAS(NAS nasAdapter)
        {

            //We will do Data Extraction Validation here
            //TODO Assume we get following information from NAS
            //var data = ResultTestData.GetRHSResultFileData();

            // return JsonConvert.SerializeObject(new { resultSet = data });

           // _applyDataExtractionRules.ApplyDataExtractionRules(nasAdapter);

            return "VisitNAS";
        }

        //public string VisitOracle(Oracle oracleAdapter)
        //{
        //    // var data = ResultTestData.GetLHSResultFileData();

        //    // return JsonConvert.SerializeObject(new { resultSet = data });
        //    return "VisitOracle";
        //}

        public string VisitSql(Sql sqlAdapter)
        {
            //We will do Data Extraction Validation here
            //TODO Assume we get following information from sql server
            // var data = ResultTestData.GetLHSResultFileData();

            // return JsonConvert.SerializeObject(new { resultSet = data });

            //TODO I need to retun extracted result from database from here and then need to Serialize
          //  _applyDataExtractionRules.ApplyDataExtractionRules(sqlAdapter);
            return "VisitSql";
        }

        public string VisitUnix(Unix unixAdapter)
        {
            return "VisitUnix";
        }
        public string VisitECG(ECG ecgAdapter)
        {
            return "VisitECG";
        }

        public string VisitOracle(Core.Entities.ExecutionEngine.Oracle oracleAdapter)
        {
            throw new NotImplementedException();
        }
    }
}
