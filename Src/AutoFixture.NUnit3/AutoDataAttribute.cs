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
                var parameters = method.GetParameters();

                var parameterValues = this.GetParameterValues(parameters);

                return new TestCaseParameters(parameterValues.ToArray());
            }
            catch (Exception ex)
            {
                return new TestCaseParameters(ex);
            }
        }

        private IEnumerable<object> GetParameterValues(IEnumerable<IParameterInfo> parameters)
        {
            return parameters.Select(Resolve);
        }

        private object Resolve(IParameterInfo parameterInfo)
        {
            CustomizeFixtureByParameter(this._fixture, parameterInfo);

            return new SpecimenContext(this._fixture)
                .Resolve(parameterInfo.ParameterInfo);
        }

        private void CustomizeFixtureByParameter(IFixture fixture, IParameterInfo parameter)
        {
            var customizeAttributes = parameter.GetCustomAttributes<CustomizeAttribute>(false);
            foreach (var ca in customizeAttributes)
            {
                var customization = ca.GetCustomization(parameter.ParameterInfo);
                fixture.Customize(customization);
            }
        }
    } 
}