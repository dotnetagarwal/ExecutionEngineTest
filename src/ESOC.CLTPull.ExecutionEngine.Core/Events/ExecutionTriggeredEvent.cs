namespace ESOC.CLTPull.ExecutionEngine.Core.Events
{
    using System;
    public class ExecutionTriggeredEvent
    {
        public ExecutionTriggeredEvent()
        {
            this.Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
        public string Message { get; set; }
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}
