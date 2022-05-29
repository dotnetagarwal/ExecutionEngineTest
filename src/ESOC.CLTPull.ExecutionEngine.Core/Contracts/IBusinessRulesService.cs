using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Core.Contracts
{
    public interface IBusinessRulesService
    {
        List<BusinessRulesInfo> GetBusinessRules(string controlType);
    }
}
