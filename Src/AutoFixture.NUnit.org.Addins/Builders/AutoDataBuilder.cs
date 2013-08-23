using System;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Ploeh.AutoFixture.NUnit.org.Addins.Builders
{
    public class AutoDataBuilder : ITestCaseBuilder2
    {
        #region ITestCaseBuilder Methods

        /// <summary>
        /// Determines if the method can be used to build an NUnit test
        /// test method of some kind. The method must normally be marked
        /// with an identifying attriute for this to be true. If the test
        /// config file sets AllowOldStyleTests to true, then any method beginning 
        /// "test..." (case-insensitive) is treated as a test unless 
        /// it is also marked as a setup or teardown method.
        /// 
        /// Note that this method does not check that the signature
        /// of the method for validity. If we did that here, any
        /// test methods with invalid signatures would be passed
        /// over in silence in the test run. Since we want such
        /// methods to be reported, the check for validity is made
        /// in BuildFrom rather than here.
        /// </summary>
        /// <param name="method">A MethodInfo for the method being used as a test method</param>
        /// <param name="suite">The test suite being built, to which the new test would be added</param>
        /// <returns>True if the builder can create a test case from this method</returns>
        public bool CanBuildFrom(MethodInfo method)
        {
            return Reflect.HasAttribute(method, Constants.AutoDataAttribute, false);
        }

        /// <summary>
        /// Build a Test from the provided MethodInfo. Depending on
        /// whether the method takes arguments and on the availability
        /// of test case data, this method may return a single test
        /// or a group of tests contained in a ParameterizedMethodSuite.
        /// </summary>
        /// <param name="method">The MethodInfo for which a test is to be built</param>
        /// <param name="suite">The test fixture being populated, or null</param>
        /// <returns>A Test representing one or more method invocations</returns>
        public Test BuildFrom(MethodInfo method)
        {
            return BuildFrom(method, null);
        }

        #endregion

        #region ITestCaseBuilder2 Members

        public bool CanBuildFrom(MethodInfo method, Test parentSuite)
        {
            return CanBuildFrom(method);
        }

        public Test BuildFrom(MethodInfo method, Test parentSuite)
        {
            var attribute = method.GetCustomAttribute<AutoDataAttribute>();

            var values = attribute.GetData(method, null);

            return new NUnitTestMethod(method);
        }

        #endregion
    }
}