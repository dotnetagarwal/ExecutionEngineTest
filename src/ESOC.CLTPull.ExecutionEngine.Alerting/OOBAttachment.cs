using ESOC.CLTPull.ExecutionEngine.Core.Constants;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.Common;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace ESOC.CLTPull.ExecutionEngine.Alerting
{
    public class OOBAttachment : IOOBAttachment
    {
        private readonly ILogger<IOOBAttachment> _logger;

        public OOBAttachment(ILogger<IOOBAttachment> logger)
        {
            _logger = logger;
        }
        public string CreateAttachment(ControlConfigurationDetail controlConfigurationDetails, ResultSetComparisonReport comparisonReport)
        {
            try
            {
                MoveToArchive(controlConfigurationDetails.ControlInformation.ControlId);
                string excelfilepath=string.Empty;
                CreateFilePath(controlConfigurationDetails, out string filepath);
                int currentRow = 0;
                MemoryStream memoryStream = new MemoryStream();
                XSSFWorkbook workbook = new XSSFWorkbook();
                XSSFSheet sheet;
                sheet = workbook.CreateSheet("OOB") as XSSFSheet;
                sheet.CreateRow(0);
                sheet.GetRow(currentRow).CreateCell(0).SetCellValue("Monitoring Date/Lookback Date");
                sheet.GetRow(currentRow).CreateCell(1).SetCellValue("Out of Balance Record");
                sheet.GetRow(currentRow).CreateCell(2).SetCellValue("Application (LHS or RHS)");
                ICellStyle cellstyle = workbook.CreateCellStyle();
                cellstyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                cellstyle.FillPattern = FillPattern.SolidForeground;
                foreach (ICell item in sheet.GetRow(0).Cells)
                {
                    item.CellStyle = cellstyle;
                    sheet.AutoSizeColumn(item.ColumnIndex);
                }
                currentRow++;
                StoreDataInExcel(controlConfigurationDetails, comparisonReport, currentRow, sheet);
                using (FileStream newfileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(newfileStream);
                    memoryStream.Close();
                    excelfilepath = filepath;
                    _logger.LogInformation("Creation completed for OOB Attachment File: {0} ", excelfilepath);
                }
                return excelfilepath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.Message);
                throw;
            }
        }
        private static void CreateFilePath(ControlConfigurationDetail controlConfigurationDetails, out string filepath)
        {          
            string controlFolder = Path.Combine(AttachmentDetails.AttachmentFolder, controlConfigurationDetails.ControlInformation.ControlId);
            if (!Directory.Exists(controlFolder))
            {
                Directory.CreateDirectory(controlFolder);
            }
            filepath = Path.Combine(controlFolder, AttachmentDetails.AttachmentName + "_" + DateTime.Now.ToString("yyyyMMdd'T'HHmmss") + "" + AttachmentDetails.ExcelExtension);
        }
        private static void StoreDataInExcel(ControlConfigurationDetail controlConfigurationDetails, ResultSetComparisonReport comparisonReport, int currentRow, XSSFSheet sheet)
        {
            for (int i = 0; i < comparisonReport.LHSOnly.Count; i++)
            {
                sheet.CreateRow(currentRow);
                sheet.GetRow(currentRow).CreateCell(0).SetCellValue(DateTime.Now.AddDays(Convert.ToInt32(controlConfigurationDetails.ControlInformation.LookBackDate)).Date);
                sheet.GetRow(currentRow).CreateCell(1).SetCellValue(comparisonReport.LHSOnly[i].Key);
                sheet.GetRow(currentRow).CreateCell(2).SetCellValue(controlConfigurationDetails.RHS.ApplicationName);
                currentRow++;
            }
        }
        public void MoveToArchive(string ControlId)
        {
            string controlFolder = Path.Combine(AttachmentDetails.AttachmentFolder, ControlId);
            string ArchiveFolder = Path.Combine(controlFolder, AttachmentDetails.ArchiveFolder);
            if (!Directory.Exists(ArchiveFolder))
                Directory.CreateDirectory(ArchiveFolder);
            DirectoryInfo controlDirectory = new DirectoryInfo(controlFolder);
            FileInfo[] attachmentFiles = controlDirectory.GetFiles();
            foreach (FileInfo file in attachmentFiles)
            {
                string fileInArchive = Path.Combine(ArchiveFolder, file.Name);
                if (File.Exists(fileInArchive))
                {
                    File.Delete(fileInArchive);
                    _logger.LogInformation("Duplicate file {0} deleted", fileInArchive);
                }
                file.MoveTo(fileInArchive);
                _logger.LogInformation("{0} moved to Archive Folder: {1}",file.Name, ArchiveFolder);
            }
        }
    }
}