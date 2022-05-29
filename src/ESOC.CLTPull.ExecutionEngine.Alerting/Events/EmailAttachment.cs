using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Alerting.Events
{
    public class EmailAttachment
    {
        public int EmailAttachmentId { get; set; }
        public DateTime LookbackDate { get; set; }
        public string  OOBRecord { get; set; }
        public string ApplicationName { get; set; }
    }
}
