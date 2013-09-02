using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit
{
    /// <summary>
    /// Abstract attribute which represents a testcase provider for a testcase.
    ///             TestCase providers derive from this attribute and implement GetArguments
    ///             to return the arguments for the testcase.
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]   
    public abstract class TestCaseDataAttribute : Attribute
    {
        /// <summary>
        /// Returns the composition of arguments to be used to test the testcase. Favors the arguments returned
        /// by TestCaseDataAttributes in ascending order.
        /// </summary>
        /// <param name="method">The method that is being tested.</param>
        /// <returns>
        /// Returns the composition of the testcase arguments.
        /// </returns>
        public abstract IEnumerable<object[]> GetArguments(MethodInfo method);
    }
}