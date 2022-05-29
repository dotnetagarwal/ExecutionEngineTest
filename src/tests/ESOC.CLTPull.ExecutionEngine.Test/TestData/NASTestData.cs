using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Test.TestData
{
    public class NASTestData
    {
        public static NAS  GetNASTestData(string[] paths)
        {
            return new NAS()
            {
                UserName = "",
                Password = "",
                FolderPaths = new Folderpaths()
                {
                    FolderPath=paths
                },
                FileIdentification=new Fileidentification()
                {
                    Time=new Time()
                    {
                        FileFromDate="0",
                        FileToDate="0"
                    },
                    //FileNamePattern= "TestFile.txt"
                }
            };
        }
    }
}
