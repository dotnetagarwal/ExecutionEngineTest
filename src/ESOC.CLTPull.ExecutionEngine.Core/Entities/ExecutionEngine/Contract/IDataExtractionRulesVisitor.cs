using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine
{
   public interface IDataExtractionRulesVisitor
    {
        string VisitNAS(NAS nasAdapter);
        string VisitUnix(Unix unixAdapter);
        string VisitOracle(Oracle oracleAdapter);
        string VisitSql(Sql sqlAdapter);
        string VisitECG(ECG ecgAdapter);
    }
}
