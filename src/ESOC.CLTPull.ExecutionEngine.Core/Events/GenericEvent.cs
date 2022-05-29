using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Core.Events
{
    public class GenericEvent
    {
        public GenericEvent()
        {
            this.Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
        public string Message { get; set; }
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }

    }
}
