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
    /// This attribute acts as a TestCaseAttribute but allow incomplete parameter values, 
    /// which will be provided by AutoFixture. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [CLSCompliant(false)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class InlineAutoDataAttribute : Attribute, ITestBuilder
    {
        private readonly object[] existingParameterValues;
        private readonly Lazy<IFixture> fixtureLazy;
        private IFixture Fixture => this.fixtureLazy.Value;

        [SuppressMessage("Performance", "CA1823:Avoid unused private fields",
            Justification = "False positive - request property is used. Bug: https://github.com/dotnet/roslyn-analyzers/issues/1321")]
        private ITestMethodBuilder testMethodBuilder = new FixedNameTestMethodBuilder();
        
        /// <summary>
        /// Gets or sets the current <see cref="ITestMethodBuilder"/> strategy.
        /// </summary>
        public ITestMethodBuilder TestMethodBuilder
        {
            get => this.testMethodBuilder;
            set => this.testMethodBuilder = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Construct a <see cref="InlineAutoDataAttribute"/>
        /// with parameter values for test method
        /// </summary>
        public InlineAutoDataAttribute(params object[] arguments)
            : this(() => new Fixture(), arguments)
        {
        }

        /// <summary>
        /// Construct a <see cref="InlineAutoDataAttribute"/> with an <see cref="IFixture"/> 
        /// and parameter values for test method
        /// </summary>
        [Obsolete("This constructor overload is deprecated because it offers poor performance, and will be removed in a future version. " +
                  "Please use the overload with a factory method, so fixture will be constructed only if needed.")]
        protected InlineAutoDataAttribute(IFixture fixture, params object[] arguments)
        {
            if (null == fixture) throw new ArgumentNullException(nameof(fixture));
            if (null == arguments) throw new ArgumentNullException(nameof(arguments));

            this.fixtureLazy = new Lazy<IFixture>(() => fixture, LazyThreadSafetyMode.None);
            this.existingParameterValues = arguments;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDataAttribute"/> class
        /// with the supplied <paramref name="fixtureFactory"/>. Fixture will be created
        /// on demand using the provided factory.
        /// </summary>
        protected InlineAutoDataAttribute(Func<IFixture> fixtureFactory, params object[] arguments)
        {
            if (null == fixtureFactory) throw new ArgumentNullException(nameof(fixtureFactory));
            if (null == arguments) throw new ArgumentNullException(nameof(arguments));

            this.fixtureLazy = new Lazy<IFixture>(fixtureFactory, LazyThreadSafetyMode.PublicationOnly);
            this.existingParameterValues = arguments;
        }

        /// <summary>
        /// Gets the parameter values for the test method.
        /// </summary>
        public IEnumerable<object> Arguments => this.existingParameterValues;

        /// <summary>
        ///     Construct one or more TestMethods from a given MethodInfo,
        ///     using available parameter data.
        /// </summary>
        /// <param name="method">The MethodInfo for which tests are to be constructed.</param>
        /// <param name="suite">The suite to which the tests will be added.</param>
        /// <returns>One or more TestMethods</returns>
        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            var test = this.TestMethodBuilder.Build(
                method, suite, this.GetParameterValues(method), this.existingParameterValues.Length);

            yield return test;
        }

        /// <summary>
        /// Get values for a collection of <see cref="IParameterInfo"/>
        /// </summary>
        private IEnumerable<object> GetParameterValues(IMethodInfo method)
        {
            var parameters = method.GetParameters();
            return this.existingParameterValues.Concat(this.GetMissingValues(parameters));
        }

        private IEnumerable<object> GetMissingValues(IEnumerable<IParameterInfo> parameters)
        {
            var parametersWithoutValues = parameters.Skip(this.existingParameterValues.Length);

            return parametersWithoutValues.Select(this.GetValueForParameter);
        }

        /// <summary>
        /// Get value for an <see cref="IParameterInfo"/>
        /// </summary>
        private object GetValueForParameter(IParameterInfo parameterInfo)
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