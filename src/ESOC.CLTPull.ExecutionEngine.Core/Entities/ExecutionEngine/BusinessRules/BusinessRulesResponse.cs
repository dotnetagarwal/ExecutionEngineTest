using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine
{
    public class BusinessRulesResponse
    {
        public bool IsSuccess { get; set; }
        public string Data { get; set; }
        public string ErrorMessage { get; set; }
    }
}
