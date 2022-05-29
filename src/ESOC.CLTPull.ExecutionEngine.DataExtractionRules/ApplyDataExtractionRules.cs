using System;
using System.Collections.Generic;
using System.Text;

namespace ESOC.CLTPull.ExecutionEngine.DataExtractionRules
{
   public class ApplyDataExtractionRules
    {
        public T CastAdapterInformationToRespectiveDataAdapter<T>(dynamic adapterInfo) where T : class
        {
            T dataAdapter = adapterInfo as T;
            if (null == dataAdapter)
                throw new ApplicationException("sqlDataAdapter objcet not able to cast"); //TODO Move it to Common Exception Handler
            return dataAdapter;
        }
    }
}
