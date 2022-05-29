using System;

namespace ESOC.CLTPull.SchedulingEngine.Core.Entities
{
    public class ControlParentInformation
    {
        public int ControlParentInformationId { get; set; }
        public string MasterId { get; set; }
        public string ParentControlId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
