using System.Collections.Generic;

namespace ESOC.CLTPull.ExecutionEngine.Core.Models
{
    public class ControlEventData
    {


        public string Name { get; set; }
        public string ControlId { get; set; }
        public List<Source> Sources { get; set; }

    }
}
