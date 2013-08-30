using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit
{
    /// <summary>
    /// Abstract attribute which represents a TestCase Provider for a TestCase.
    ///             TestCase providers derive from this attribute and implement GetArguments
    ///             to return the arguments for the TestCases.
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]   
    public abstract class ArgumentsAttribute : Attribute
    {
        /// <summary>
        /// Returns the composition of arguments to be used to test the TestCase. Favors the arguements returned
        /// by ArguementsAttributes in ascending order. 
        /// </summary>
        /// <param name="method">The method that is being tested.</param>
        /// <param name="parameterTypes">The types of the parameters for the test method.</param>
        /// <returns>
        /// Returns the composition of the TestCase provider.
        /// </returns>
        /// <remarks>
        /// The number of test cases is set from the first ArgumentsAttribute TestCase length.
        /// </remarks>
        public abstract IEnumerable<object[]> GetArguments(MethodInfo method, Type[] parameterTypes);
    }
}