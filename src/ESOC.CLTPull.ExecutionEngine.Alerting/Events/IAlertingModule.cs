using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Alerting.Events
{
  public  interface IAlertingModule
    {
         void SendResultToAlertingModule(dynamic resultEvent);
    }
}
