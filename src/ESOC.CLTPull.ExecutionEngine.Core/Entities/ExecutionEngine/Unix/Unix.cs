using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine
{
    public class Unix : IDataExtractionRules
    {
        Task<string> IDataExtractionRules.AcceptDataExtractionRules(dynamic adapterHandler)
        {
            throw new NotImplementedException();
        }
    }
}
