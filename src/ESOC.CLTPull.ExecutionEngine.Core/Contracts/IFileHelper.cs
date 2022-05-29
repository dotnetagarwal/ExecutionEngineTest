using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Core.Contracts
{
    public interface IFileHelper
    {
        public string ConnectToMachine();
        public string ConnectToPath();
        public string ExecuteCommand();


    }
}
