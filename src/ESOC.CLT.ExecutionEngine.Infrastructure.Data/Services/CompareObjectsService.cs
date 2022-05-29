using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data
{
   public class CompareObjectsService : ICompareObjectsService
    {

        public async Task<ResultSetComparisonReport> CompareTwoResultSets(JObject lhs, JObject rhs, string rootNode)
        {
            ResultSetComparisonReport comparisonReport = new ResultSetComparisonReport();
            JObject differetial = new JObject();
            JObject lhsObject = (JObject)lhs["resultSet"];
            JObject rhsObject = (JObject)rhs["resultSet"];
            int rhsCount = rhs["resultSet"].Count();
            bool isMatchFound = false;
            foreach (KeyValuePair<string, JToken> lhsKeyVal in lhsObject)
            {
                foreach (KeyValuePair<string, JToken> rhsKeyVal in rhsObject.ToObject<JObject>())
                {
                    isMatchFound = false;
                    rhsCount--;
                    if (lhsKeyVal.Key != rhsKeyVal.Key)//TODO WE need TO compare for types as well
                    {
                        if (rhsCount == 0)
                        {
                            //Add LHS Current Element into differential JObject as corresponding RHS Element not found
                            comparisonReport.LHSOnly.Add(lhsKeyVal);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    isMatchFound = true;

                    var jTokenType = rhsKeyVal.Value.Type;
                    switch (jTokenType)
                    {
                        //If 
                        case JTokenType.Object:
                            var jObject = (JObject)rhsKeyVal.Value;
                            break;

                        case JTokenType.Array:
                            rhsObject.Remove(rhsKeyVal.Key); //Will remove JArray key ireespective of comparison, as those we are tracking separately
                            await CompareJArrays((JArray)lhsKeyVal.Value, (JArray)rhsKeyVal.Value, comparisonReport);
                            break;

                        case JTokenType.Integer:
                            rhsObject.Remove(rhsKeyVal.Key);
                            if (JToken.DeepEquals(lhsKeyVal.Value, rhsKeyVal.Value))
                            {
                                //TO DO if match found

                            }
                            break;

                        case JTokenType.String:
                            rhsObject.Remove(rhsKeyVal.Key);
                            if (JToken.DeepEquals(lhsKeyVal.Value, rhsKeyVal.Value))
                            {
                                //TODO if match found

                            }
                            break;
                    }
                    if (isMatchFound)
                        break; //If match found, then move lhs to next element
                }
            }

            //Add remaining element of RHS into RHSOnly
            foreach (KeyValuePair<string, JToken> rhsKeyVal in rhsObject.ToObject<JObject>())
            {

                comparisonReport.RHSOnly.Add(rhsKeyVal);
            }

            return comparisonReport;
        }
        //Assuming both the Arrays of same type
        public async Task<ResultSetComparisonReport> CompareJArrays(JArray lhs, JArray rhs, ResultSetComparisonReport comparisonReport)
        {
            var firstChildrenName = ((Newtonsoft.Json.Linq.JProperty)lhs[0].Children().First()).Name;
            //TODO Needs to see if lhs and rhs both will have same element at first place
            lhs = new JArray { lhs.OrderBy(e => e[firstChildrenName]).ToArray() };
            rhs = new JArray { rhs.OrderBy(e => e[firstChildrenName]).ToArray() };

            for (int i = 0, n = Math.Min(lhs.Count, rhs.Count); i < n; i++)
            {
                bool isEqual = JToken.DeepEquals(lhs[i], rhs[i]);
                if (!isEqual)
                {
                    comparisonReport.LHSDifferential.Add((JObject)lhs[i]);
                    comparisonReport.RHSDifferential.Add((JObject)rhs[i]);
                }
            }
            return await Task.Run(()=> comparisonReport);
        }
    }

   
}
