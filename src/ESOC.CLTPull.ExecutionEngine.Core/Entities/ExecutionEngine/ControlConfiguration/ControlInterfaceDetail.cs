using System;

namespace ESOC.CLTPull.SchedulingEngine.Core.Entities
{
    public class ControlInterfaceDetail
    {
        public int InterfaceDetailId{ get; set; }
        public int? ControlParentInformationId { get; set; }
        public bool IsLHS { get; set; }
        public string AdapterType { get; set; }
        public string InterfaceConfigurationDetail { get; set; }
        public int? SecurityCredentialId { get; set; }
        public DateTime? ValidTill { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
