using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3
{
    /// <summary>
    /// This attribute acts as a <see cref="TestCaseSourceAttribute" /> but allow incomplete parameter values,
    /// which will be provided by AutoFixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [CLSCompliant(false)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
        Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class AutoTestCaseSourceAttribute : Attribute, ITestBuilder, IImplyFixture
    {
        /// <summary>
        /// The Fixture factory.
        /// </summary>
        protected Func<IFixture> FixtureFactory { get; set; }

        /// <summary>
        /// Creates a new attribute instance.
        /// </summary>
        /// <param name="sourceName">The name of the source member.</param>
        /// <param name="parameters">
        /// The collection of parameters passed to the source member.
        /// Ignored if the source member is not a method.
        /// </param>
        public AutoTestCaseSourceAttribute(string sourceName, params object[] parameters)
            : this(() => new Fixture(), default, sourceName, parameters)
        {
        }

        /// <summary>
        /// Creates a new attribute instance.
        /// </summary>
        /// <param name="sourceType">The test case source type.</param>
        public AutoTestCaseSourceAttribute(Type sourceType)
            : this(() => new Fixture(), sourceType, default, default)
        {
        }

        /// <summary>
        /// Creates a new attribute instance.
        /// </summary>
        /// <param name="sourceType">The type holding the test case source member.</param>
        /// <param name="sourceName">The name of the source member.</param>
        /// <param name="parameters">
        /// The collection of parameters passed to the source member.
        /// Ignored if the source member is not a method.
        /// </param>
        public AutoTestCaseSourceAttribute(Type sourceType, string sourceName, params object[] parameters)
            : this(() => new Fixture(), sourceType, sourceName, parameters)
        {
        }

        /// <summary>
        /// Creates a new attribute instance.
        /// </summary>
        /// <param name="fixtureFactory">The factory for creating new Fixture instances.</param>
        /// <param name="sourceType">The type holding the test case source member.</param>
        /// <param name="sourceName">The name of the source member.</param>
        /// <param name="parameters">
        /// The collection of parameters passed to the source member.
        /// Ignored if the source member is not a method.
        /// </param>
        protected AutoTestCaseSourceAttribute(Func<IFixture> fixtureFactory,
            Type sourceType, string sourceName, IReadOnlyList<object> parameters)
        {
            this.FixtureFactory = fixtureFactory ?? throw new ArgumentNullException(nameof(fixtureFactory));
            this.SourceName = sourceName;
            this.SourceType = sourceType;
            this.Parameters = parameters;
        }

        /// <summary>
        /// The name of a the method, property or fiend to be used as a source.
        /// </summary>
        public string SourceName { get; }

        /// <summary>A Type to be used as a source.</summary>
        public Type SourceType { get; }

        /// <summary>
        /// Gets the values passed to the source, if the source member is a method.
        /// </summary>
        public IReadOnlyList<object> Parameters { get; }

        /// <summary>
        /// Gets or sets the category associated with every fixture created from
        /// this attribute. May be a single category or a comma-separated list.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Construct one or more TestMethods from a given MethodInfo, using available parameter data.
        /// </summary>
        /// <param name="method"> The IMethod for which tests are to be constructed.</param>
        /// <param name="suite">The suite to which the tests will be added.</param>
        /// <returns>One or more TestMethods.</returns>
        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            var source = new ReflectedSource(
                this.SourceType ?? method.TypeInfo.Type,
                this.SourceName,
                this.Parameters);

            var builder = new AutoTestCaseBuilder();

            foreach (var values in source.GetTestCases(method))
            {
                var parameters = new AutoTestCaseParameters(this.FixtureFactory, values)
                {
                    Category = this.Category
                };

                yield return builder.BuildTestMethod(method, suite, parameters);
            }
        }
    }
}
