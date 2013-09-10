using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit
{
    /// <summary>
    /// Static class to hold the results of AutoFixture parameter generation so that it 
    /// only gets generated once for each test run.
    /// </summary>
    public static class ParameterStore
    {
        private static readonly ConcurrentDictionary<string, object> _parameterStore = 
            new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Function that will either return the value of a Func or return the previously calculated value.
        /// </summary>
        /// <param name="typeName">Type name of the class.</param>
        /// <param name="parameterInfo">Parameter info to store information for.</param>
        /// <param name="func">Function to generate a value based on the parameter info.</param>
        /// <returns>The original result of func(parameterInfo).</returns>
        public static object GetOrAdd(string typeName, ParameterInfo parameterInfo, Func<ParameterInfo,object> func)
        {
            string key = typeName + "_" + parameterInfo.Member.Name + "_" + parameterInfo.Position;
            return _parameterStore.GetOrAdd(key, k => func(parameterInfo));
        }
    }
}