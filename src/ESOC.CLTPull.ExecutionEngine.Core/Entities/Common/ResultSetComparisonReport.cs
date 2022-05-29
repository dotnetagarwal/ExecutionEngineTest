using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ESOC.CLTPull.ExecutionEngine.Core.Entities.Common
{
    public class ResultSetComparisonReport
    {
        public ResultSetComparisonReport()
        {
            LHSDifferential = new List<JObject>();
            RHSDifferential = new List<JObject>();
            LHSOnly = new List<KeyValuePair<string, JToken>>();
            RHSOnly = new List<KeyValuePair<string, JToken>>();
        }


        public List<JObject> LHSDifferential { get; set; }
        public List<JObject> RHSDifferential { get; set; }
        public List<KeyValuePair<string, JToken>> LHSOnly { get; set; }
        public List<KeyValuePair<string, JToken>> RHSOnly { get; set; }
    }
}
