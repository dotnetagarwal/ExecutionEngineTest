using System;

namespace ESOC.CLTPull.ExecutionEngine.Core.Models
{
    public class ControlExecutionEvent
    {

        public ControlExecutionEvent()
        {
            this.Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public string Message { get; set; }
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }

    }
}
