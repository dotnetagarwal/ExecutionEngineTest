using System;

namespace ESOC.CLTPull.SchedulingEngine.Core.Entities
{
    public class SecurityCredential
    {
        public int SecurityCredentialID { get; set; }
        public string SecurityCredentialName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
