using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine
{
    public class NAS : IDataExtractionRules
    {
        private readonly INASApplyDataExtractionRules _nasApplyDataExtractionRules;
        private readonly INASConnection _nasConnection;

        public NAS(INASApplyDataExtractionRules nasApplyDataExtractionRules,INASConnection nasConnection)
        {
            _nasApplyDataExtractionRules = nasApplyDataExtractionRules;
            _nasConnection = nasConnection;
        }

        public NAS()
        {

        }

        ////public NAS()
        ////{
        ////    _nasApplyDataExtractionRules = new TempNASApplyDataExtractionRules();
        ////}

        public string UserName { get; set; }
        public string Password { get; set; }
        public Folderpaths FolderPaths { get; set; }
        public Fileidentification FileIdentification { get; set; }

        public NASDataFetchRules DataFetchRules { get; set; }

        //This is Accept Method Of Visitor Pattern
        public async Task<string> AcceptDataExtractionRules(dynamic adapterHandler)
        {
            //Add NAS code 
            // "I will apply all NAS Data Extraction Rules here";

            return await adapterHandler.ApplyDataExtractionRules(this);

            // return dataExtractionRulesVisitor.VisitNAS(this);
        }
    }

    public class NASDataFetchRules
    {
        public Filerules FileRules { get; set; }
    }

    public class Filerules
    {
        public string IsDownloadFile { get; set; }
        public string IsInZipFormat { get; set; }
        public string GetFileCount { get; set; }
        public string FileDataSerachPattern { get; set; }
        public string GetRowCount { get; set; }
    }

    public class Folderpaths
    {
        public string[] FolderPath { get; set; }
    }

    public class Fileidentification
    {
        public Time Time { get; set; }
        public Fileattributes FileAttributes { get; set; }
        public FileNamePattern FileNamePattern { get; set; }
        public FileType FileType { get; set; }
    }

    public class FileNamePattern
    {
        public string[] Absolute { get; set; }
        public string DatePattern { get; set; }
    }

    public class Time
    {
        public string FileFromDate { get; set; }
        public string FileToDate { get; set; }
        public string TimeSpanInHours { get; set; }
        public string LookbackDate { get; set; }
        public string LookbackSpan { get; set; }
    }

    public class Fileattributes
    {
        public string LastModifiedDate { get; set; }
        public string FileArrivalTime { get; set; }
    }

    public class ResultData
    {
        public string[] Data { get; set; }
    }

    public class FileType
    {
        public CSV CSV { get; set; }
    }

    public class CSV
    {
        public string ColumnName { get; set; }
    }
}
