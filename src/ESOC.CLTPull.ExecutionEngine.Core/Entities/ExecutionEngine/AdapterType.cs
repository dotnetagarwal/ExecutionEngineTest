using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine
{
   public class AdapterType
    {
        public Sql Sql { get; set; }
        public NAS NAS { get; set; }
        public Oracle Oracle { get; set; }
        public ECG ECG { get; set; }
        public Unix Unix { get; set; }
    }
}
