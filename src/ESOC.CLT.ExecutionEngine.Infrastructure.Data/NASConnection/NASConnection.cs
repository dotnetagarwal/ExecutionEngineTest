using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data
{
    public class NASConnection: INASConnection
    {
        private readonly ILogger<NASConnection> _logger;

        public NASConnection(ILogger<NASConnection> logger)
        {
            _logger = logger;
        }
        public async Task<string[]> GetFileData(NAS adapterInfo)
        {
            Dictionary<string, object> patterns = ControlExecutionResultsService.GetPatternTypes(adapterInfo.FileIdentification.FileNamePattern);

            string date = patterns[ControlEnum.Pattern.DatePattern.ToString()].ToString();
            List<string> patternArray = ((IEnumerable)patterns[ControlEnum.Pattern.Absolute.ToString()]).Cast<object>().Select(x => x.ToString()).ToList();

            var currentDate = DateTime.Now;
            var days = Int32.Parse(adapterInfo.FileIdentification.Time.LookbackDate);
            string dateText = currentDate.AddDays(days).ToString(date);
            patternArray.Add(dateText);

            var nasData = FilterFileData(adapterInfo, patternArray);
            var data = nasData.ToArray();
            return data;
        }

        private List<string> FilterFileData(NAS adapterInfo, List<string> patternArray)
        {
            string[] folderpaths = adapterInfo.FolderPaths.FolderPath;
            List<string> nasData = new List<string>();
            foreach (string path in folderpaths)
            {
                using (AxisImpersonate.AxisImpersonate impersonate = new AxisImpersonate.AxisImpersonate())
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        int count = impersonate.ConnectToServer(path, adapterInfo.UserName, "ms", adapterInfo.Password);
                        _logger.LogInformation($"Response code for NAS connection: {count}");
                        _logger.LogInformation($"NAS path: {path}");
                        DirectoryInfo dir = new DirectoryInfo(path);
                        _logger.LogInformation($"Directory info: {dir.FullName}");
                        var files = dir.GetFiles().Where(x => x.LastWriteTime.Date >= DateTime.Now.AddDays(Int32.Parse(adapterInfo.FileIdentification.Time.FileFromDate)) &&
                        x.LastWriteTime.Date <= DateTime.Now.AddDays(Int32.Parse(adapterInfo.FileIdentification.Time.FileToDate))).ToList();
                        _logger.LogInformation($"Numberof files at NAS: {files.Count}");
                        foreach (var text in patternArray)
                        {
                            files = files.Where(x => x.Name.Contains(text)).ToList();
                        }
                        if (files.Count > 0)
                           nasData= FetchDataFromCSV(files, adapterInfo.FileIdentification.FileType);
                    }
                }
            }
            return nasData;
        }

        private List<string> FetchDataFromCSV(List<FileInfo> files, FileType fileType)
        {
            DataTable csvData = new DataTable();
            csvData.CSVtoDataTable(files.FirstOrDefault().FullName);
            return csvData.AsEnumerable()
               .Select(r => r.Field<string>(fileType.CSV.ColumnName).Replace("\r", ""))
               .ToList();
        }
    }
}
