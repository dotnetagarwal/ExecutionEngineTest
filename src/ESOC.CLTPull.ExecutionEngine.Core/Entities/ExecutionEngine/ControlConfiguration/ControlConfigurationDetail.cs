using System;

namespace ESOC.CLTPull.SchedulingEngine.Core.Entities
{
    public class ControlConfigurationDetail
    {
        public int ControlConfigurationId { get; set; }
        public int? ControlParentInformationId { get; set; }
        public string ConfigurationDetails { get; set; }
        public DateTime? ValidTill { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
