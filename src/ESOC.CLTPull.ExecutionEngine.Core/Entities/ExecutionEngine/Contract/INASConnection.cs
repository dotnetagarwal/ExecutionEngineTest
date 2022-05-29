using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract
{
    public interface INASConnection
    {
        Task<string[]> GetFileData(NAS adapterInfo);
    }
}
