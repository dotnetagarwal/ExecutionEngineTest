using System;

namespace ESOC.CLTPull.ExecutionEngine.Alerting.Events
{
    public class RecordCountEvent
    {
        public string MasterID { get; set; }
        public string RecordOriginatingSource { get; set; }
        public string ControlDescription { get; set; }
        public DateTime LookbackDate { get; set; }
        public DateTime ControlExecutionDate { get; set; }
        public int SourceCount { get; set; }
        public int DestinationCount { get; set; }
        public int OOBCount { get; set; }
        public string ExecutionResult { get; set; }
        public string AttachmentPath { get; set; }
        public int? TicketingCaseId { get; set; }
    }
}
