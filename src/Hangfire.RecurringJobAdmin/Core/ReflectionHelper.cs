using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hangfire.RecurringJobAdmin.Core
{
    public static class ReflectionHelper
    {

        public static void InvokeVoidMethod(string typeName, string methodName)
        {
            // Get the Type for the class
            Type calledType = Type.GetType(typeName);

            if (calledType != null)
            {
                MethodInfo methodInfo = calledType.GetMethod(methodName);

                if (methodInfo != null)
                {
                    object result = null;
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    object classInstance = Activator.CreateInstance(calledType, null);

                    if (parameters.Length == 0)
                    {
                        // This works fine
                        result = methodInfo.Invoke(classInstance, null);
                    }
                    else
                    {
                        object[] parametersArray = new object[] { "Hello" };

                        // The invoke does NOT work;
                        // it throws "Object does not match target type"             
                        result = methodInfo.Invoke(methodInfo, parametersArray);
                    }
                }
            }
        }
    }
}
