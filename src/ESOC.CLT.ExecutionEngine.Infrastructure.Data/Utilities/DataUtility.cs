using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static ESOC.CLTPull.ExecutionEngine.Core.Constants.ControlConstants;

namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data
{
    public static class DataUtility
    {
        public static DataTable CSVtoDataTable(this DataTable csvdt, string inputpath)
        {
            string Fulltext;

            if (File.Exists(inputpath))
            {
                using (StreamReader sr = new StreamReader(inputpath))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString();//read full content
                        string[] rows = Fulltext.Split('\n');//split file content to get the rows
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            var regex = new Regex("\\\"(.*?)\\\"");
                            var output = regex.Replace(rows[i], m => m.Value.Replace(",", "\\c"));//replace commas inside quotes
                            string[] rowValues = output.Split(',');//split rows with comma',' to get the column values
                            {
                                if (i == 0)
                                {
                                    for (int j = 0; j < rowValues.Count(); j++)
                                    {
                                        var scolumn = rowValues[j].Replace("\r", "");
                                        csvdt.Columns.Add(scolumn.Replace("\\c", ","));//headers
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        DataRow dr = csvdt.NewRow();
                                        for (int k = 0; k < rowValues.Count(); k++)
                                        {
                                            if (k >= dr.Table.Columns.Count)// more columns may exist
                                            {
                                                csvdt.Columns.Add("clmn" + k);
                                                dr = csvdt.NewRow();
                                            }
                                            dr[k] = rowValues[k].Replace("\\c", ",");
                                        }
                                        csvdt.Rows.Add(dr);//add other rows
                                    }
                                    catch(Exception ex)
                                    {
                                        Console.WriteLine($" {ex.Message+ex.StackTrace} ");
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return csvdt;
        }

        public static string GetOracleConnectionString(CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Oracle adapterInfo)
        {
            return String.Format(Connectionstrings.Oracle, adapterInfo.ServerName, adapterInfo.port, adapterInfo.DatabaseName, adapterInfo.UserName, adapterInfo.Password);
        }
    }
}
