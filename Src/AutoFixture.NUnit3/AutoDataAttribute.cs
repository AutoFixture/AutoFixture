using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using Ploeh.AutoFixture.Kernel;
using System.Diagnostics.CodeAnalysis;

namespace Ploeh.AutoFixture.NUnit3
{
    /// <summary>
    /// This attribute uses AutoFixture to generate values for unit test parameters. 
    /// This implementation is based on TestCaseAttribute of NUnit3
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", 
        Justification = "This attribute is the root of a potential attribute hierarchy.")]
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoDataAttribute : Attribute, ITestBuilder
    {
        private readonly IFixture _fixture;

        private ITestMethodBuilder _testMethodBuilder = new VolatileNameTestMethodBuilder();
        /// <summary>
        /// Gets or sets the current <see cref="ITestMethodBuilder"/> strategy.
        /// </summary>
        public ITestMethodBuilder TestMethodBuilder
        {
            get { return _testMethodBuilder; }
            set { _testMethodBuilder = value ?? throw new ArgumentNullException(nameof(value)); }
        }

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
            if (null == fixture)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

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
            var test = this.TestMethodBuilder.Build(method, suite, GetParameterValues(method), 0);

            yield return test;
        }

        private IEnumerable<object> GetParameterValues(IMethodInfo method)
        {
            return method.GetParameters().Select(Resolve);
        }

        private object Resolve(IParameterInfo parameterInfo)
        {
            CustomizeFixtureByParameter(parameterInfo);

            return new SpecimenContext(this._fixture)
                .Resolve(parameterInfo.ParameterInfo);
        }

        private void CustomizeFixtureByParameter(IParameterInfo parameter)
        {
            var customizeAttributes = parameter.GetCustomAttributes<CustomizeAttribute>(false)
                .OrderBy(x => x, new CustomizeAttributeComparer());

            foreach (var ca in customizeAttributes)
            {
                var customization = ca.GetCustomization(parameter.ParameterInfo);
                this._fixture.Customize(customization);
            }
        }
    }
}