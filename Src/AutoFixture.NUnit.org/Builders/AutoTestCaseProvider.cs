using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Ploeh.AutoFixture.NUnit.org.Builders
{
    /// <summary>
    /// TestCaseSourceProvider provides data for methods
    /// annotated with the TestCaseSourceAttribute.
    /// </summary>
    public class AutoTestCaseProvider : ITestCaseProvider2
    {
        #region ITestCaseProvider Members

        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        public bool HasTestCasesFor(MethodInfo method)
        {
            return Reflect.HasAttribute(method, AutoFixtureNUnitFramework.AutoTestCaseAttribute, false);
        }

        /// <summary>
        /// Return an IEnumerable providing test cases for use in
        /// running a parameterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IEnumerable GetTestCasesFor(MethodInfo method)
        {
            return GetTestCasesFor(method, null);
        }

        #endregion

        #region ITestCaseProvider2 Members

        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        public bool HasTestCasesFor(MethodInfo method, Test suite)
        {
            return HasTestCasesFor(method);
        }

        /// <summary>
        /// Return an IEnumerable providing test cases for use in
        /// running a parameterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IEnumerable GetTestCasesFor(MethodInfo method, Test parentSuite)
        {
            Type[] parameterTypes = method.GetParameters().Select(o => o.ParameterType).ToArray();
            
            ArrayList parameterList = new ArrayList();

            var attributes = Reflect.GetAttributes(method, AutoFixtureNUnitFramework.AutoTestCaseAttribute, false);

            foreach (DataAttribute attr in attributes)
            {
                foreach (var data in attr.GetData(method, parameterTypes))
                {
                    ParameterSet parms = new ParameterSet();
                    parms.Arguments = data;

                    parameterList.Add(parms);
                }
            }

            return parameterList;
        }
        #endregion
    }
}
