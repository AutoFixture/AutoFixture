using System.Collections;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Ploeh.AutoFixture.NUnit2.Addins.Builders
{
    /// <summary>
    /// AutoTestCaseProvider provides data for methods
    /// annotated with the AutoTestCaseAttribute.
    /// </summary>
    public class AutoDataProvider : ITestCaseProvider2
    {
        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        public bool HasTestCasesFor(MethodInfo method)
        {
            return Reflect.HasAttribute(method, Constants.AutoDataAttribute, false);
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

        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <param name="suite">A Suite representing a NUnit TestSuite</param>
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
        /// <param name="suite"></param>
        /// <returns></returns>
        public IEnumerable GetTestCasesFor(MethodInfo method, Test suite)
        {
            ArrayList parameterList = new ArrayList();

            var attributes = Reflect.GetAttributes(method, Constants.AutoDataAttribute, false);

            foreach (DataAttribute attr in attributes)
            {
                foreach (var arguments in attr.GetData(method))
                {
                    ParameterSet parms = new ParameterSet();
                    parms.Arguments = arguments;

                    parameterList.Add(parms);
                }
            }

            return parameterList;
        }
    }
}
