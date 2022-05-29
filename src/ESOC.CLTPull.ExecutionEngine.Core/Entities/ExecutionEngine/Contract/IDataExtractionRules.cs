using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine
{
   public interface IDataExtractionRules
    {
        //Accept method of Visitor Pattern      
        //We will return LHSResult json as string
        public Task<string> AcceptDataExtractionRules(dynamic adapterHandler);
       // public string AcceptDataExtractionRules(IDataExtractionRulesVisitor dataExtractionRulesVisitor);
    }
}
