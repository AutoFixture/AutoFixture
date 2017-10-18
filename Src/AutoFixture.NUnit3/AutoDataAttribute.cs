using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using AutoFixture.Kernel;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3
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
        private readonly Lazy<IFixture> fixtureLazy;
        private IFixture Fixture => this.fixtureLazy.Value;

        private ITestMethodBuilder _testMethodBuilder = new FixedNameTestMethodBuilder();
        /// <summary>
        /// Gets or sets the current <see cref="ITestMethodBuilder"/> strategy.
        /// </summary>
        public ITestMethodBuilder TestMethodBuilder
        {
            get { return this._testMethodBuilder; }
            set { this._testMethodBuilder = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        /// <summary>
        /// Construct a <see cref="AutoDataAttribute"/>
        /// </summary>
        public AutoDataAttribute()
            : this(() => new Fixture())
        {
        }

        /// <summary>
        /// Construct a <see cref="AutoDataAttribute"/> with an <see cref="IFixture"/> 
        /// </summary>
        /// <param name="fixture"></param>
        [Obsolete("This constructor overload is deprecated as it ins't performance efficient and will be removed in a future version. " +
                  "Please use the AutoDataAttribute(Func<IFixture> fixtureFactory) overload, so fixture will be constructed only if needed.")]
        protected AutoDataAttribute(IFixture fixture)
        {
            if (null == fixture)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            this.fixtureLazy = new Lazy<IFixture>(() => fixture, LazyThreadSafetyMode.None);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDataAttribute"/> class
        /// with the supplised <paramref name="fixtureFactory"/>. Fixture will be created
        /// on demand using the provided factory.
        /// </summary>
        /// <param name="fixtureFactory">The fixture factory used to construct the fixture.</param>
        protected AutoDataAttribute(Func<IFixture> fixtureFactory)
        {
            if (fixtureFactory == null) throw new ArgumentNullException(nameof(fixtureFactory));

            this.fixtureLazy = new Lazy<IFixture>(fixtureFactory, LazyThreadSafetyMode.PublicationOnly);
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
            var test = this.TestMethodBuilder.Build(method, suite, this.GetParameterValues(method), 0);

            yield return test;
        }

        private IEnumerable<object> GetParameterValues(IMethodInfo method)
        {
            return method.GetParameters().Select(this.Resolve);
        }

        private object Resolve(IParameterInfo parameterInfo)
        {
            this.CustomizeFixtureByParameter(parameterInfo);

            return new SpecimenContext(this.Fixture)
                .Resolve(parameterInfo.ParameterInfo);
        }

        private void CustomizeFixtureByParameter(IParameterInfo parameter)
        {
            var customizeAttributes = parameter.GetCustomAttributes<Attribute>(false)
                .OfType<IParameterCustomizationSource>()
                .OrderBy(x => x, new CustomizeAttributeComparer());

            foreach (var ca in customizeAttributes)
            {
                var customization = ca.GetCustomization(parameter.ParameterInfo);
                this.Fixture.Customize(customization);
            }
        }
    }
}