using System;
using System.Collections.Generic;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine
{
    public class BusinessRulesInfo
    {
        public String ControlType { get; set; }
        public String ControlStatus { get; set; }
        public List<String> Rules { get; set; }
    }
}
