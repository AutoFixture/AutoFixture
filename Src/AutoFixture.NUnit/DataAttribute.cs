using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit
{
    /// <summary>
    /// Abstract attribute which represents a testcase provider for a testcase.
    ///             TestCase providers derive from this attribute and implement GetData
    ///             to return the data for the testcase.
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]   
    public abstract class DataAttribute : Attribute
    {
        /// <summary>
        /// Returns the composition of data to be used to test the testcase. Favors the data returned
        /// by DataAttributes in ascending order. 
        /// </summary>
        /// <param name="method">The method that is being tested.</param>
        /// <param name="parameterTypes">The types of the parameters for the test method.</param>
        /// <returns>
        /// Returns the composition of the testcase data.
        /// </returns>
        public abstract IEnumerable<object[]> GetData(MethodInfo method, Type[] parameterTypes);
    }
}