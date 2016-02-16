using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.NUnit3
{
    /// <summary>
    /// This attribute uses AutoFixture to generate values for unit test parameters. 
    /// This implementation is based on TestCaseAttribute of NUnit3
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoDataAttribute : Attribute, ITestBuilder
    {
        private readonly IFixture _fixture;

        /// <summary>
        /// Construct a <see cref="AutoDataAttribute"/>
        /// </summary>
        public AutoDataAttribute()
            : this(new Fixture())
        {
        }

        /// <summary>
        /// Construct a <see cref="AutoDataAttribute"/> with an <see cref="IFixture"/> 
        /// </summary>
        /// <param name="fixture"></param>
        protected AutoDataAttribute(IFixture fixture)
        {
            _fixture = fixture;
        }

        /// <summary>
        ///     Construct one or more TestMethods from a given MethodInfo,
        ///     using available parameter data.
        /// </summary>
        /// <param name="method">The MethodInfo for which tests are to be constructed.</param>
        /// <param name="suite">The suite to which the tests will be added.</param>
        /// <returns>One or more TestMethods</returns>
        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            var test = new NUnitTestCaseBuilder().BuildTestMethod(method, suite, this.GetParametersForMethod(method));

            yield return test;
        }

        private TestCaseParameters GetParametersForMethod(IMethodInfo method)
        {
            try
            {
                var specimenBuilder = new SpecimenContext(this._fixture);
                var parameterValues = method.GetParameters()
                    .Select(p => specimenBuilder.Resolve(p.ParameterInfo))
                    .ToArray();
                return new TestCaseParameters(parameterValues);
            }
            catch (Exception ex)
            {
                return new TestCaseParameters(ex);
            }
        }
    } 
}