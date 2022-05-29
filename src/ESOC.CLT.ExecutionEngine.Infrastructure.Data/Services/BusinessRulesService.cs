using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace ESOC.CLT.ExecutionEngine.Infrastructure.Data.Services
{
    public class BusinessRulesService : IBusinessRulesService
    {
        public List<BusinessRulesInfo> GetBusinessRules(string controlType)
        {
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + "ESOC.CLTPull.ExecutionEngine.BusinessRules.dll";

            Assembly assesmbly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
            Dictionary<String, List<String>> typeMethodsMap = new Dictionary<string, List<string>>();
            List<BusinessRulesInfo> businessRulesInfoList = new List<BusinessRulesInfo>();
            List<String> typeMethodList = null;

            //Get List of Class Name
            Type[] types = assesmbly.GetTypes();

            //Get all rule names of the method
            foreach (Type tc in types)
            {
                if (tc.IsPublic)
                {
                    //Get List of Method Names of Class
                    MemberInfo[] methodName = tc.GetMethods(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    typeMethodList = new List<string>();
                    foreach (MemberInfo method in methodName)
                    {
                        if (method.ReflectedType.IsPublic)
                        {
                            typeMethodList.Add(method.Name);
                        }
                    }
                }
                typeMethodsMap.Add(tc.Name, typeMethodList);

                //TODO We need to break class name with "_" to find out ControlType as well as Status
                string[] controlTypeStatus = tc.Name.Split("_", StringSplitOptions.RemoveEmptyEntries);
                if (null == controlTypeStatus || controlTypeStatus.Length < 2)
                    continue;
                if (controlTypeStatus[0] != controlType)
                    continue;
                
                BusinessRulesInfo businessRulesInfo = new BusinessRulesInfo();

                businessRulesInfo.Rules = typeMethodList;
                
                businessRulesInfo.ControlType = controlTypeStatus[0];
                businessRulesInfo.ControlStatus = controlTypeStatus[1];
                businessRulesInfoList.Add(businessRulesInfo);
            }

            return businessRulesInfoList;
        }
    }
}
