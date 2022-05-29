using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.SchedulingEngine.Core.Entities
{
    public class ControlBusinessRulesDetail
    {
        public int BussinessRuleDetailId { get; set; }
        public int? ControlParentInformationId { get; set; }
        public string BusinessRulesConfigurationDetail { get; set; }
        public DateTime? ValidTill { get; set; }
        public string ReconcilationRuleType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
