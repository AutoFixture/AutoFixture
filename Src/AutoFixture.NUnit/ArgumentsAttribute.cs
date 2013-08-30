using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit
{
    /// <summary>
    /// Abstract attribute which represents a arguments provider for a testcase.
    ///             TestCase Providers derive from this attribute and implement GetArguments
    ///             to return the arguments for the testcase.
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]   
    public abstract class ArgumentsAttribute : Attribute
    {
        /// <summary>
        /// Returns the arguments to be used to test the testcase.
        /// </summary>
        /// <param name="method">The method that is being tested</param><param name="parameterTypes">The types of the parameters for the test method</param>
        /// <returns>
        /// The testcase arguments
        /// </returns>
        public abstract IEnumerable<object[]> GetArguments(MethodInfo method, Type[] parameterTypes);
    }
}