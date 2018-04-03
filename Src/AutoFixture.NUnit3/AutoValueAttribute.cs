using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using AutoFixture.Kernel;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace AutoFixture.NUnit3
{
    /// <summary>
    /// This attribute uses AutoFixture to generate values for unit test parameters.
    /// This implementation is based on DataAttribute and IParameterDataSource of NUnit3
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is the root of a potential attribute hierarchy.")]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class AutoValueAttribute : DataAttribute, IParameterDataSource
    {
        private readonly Lazy<IFixture> _fixtureLazy;
        private IFixture Fixture => _fixtureLazy.Value;

        /// <summary>
        /// Constructs a <see cref="AutoValueAttribute"/>
        /// </summary>
        public AutoValueAttribute() : this (() => new Fixture())
        {
                
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoValueAttribute"/> class
        /// with the supplied <paramref name="fixtureBuilder"/>. Fixture will be created
        /// on demand using the provided factory
        /// </summary>
        /// <param name="fixtureBuilder">The fixture factory used to construct the fixture.</param>
        protected AutoValueAttribute(Func<IFixture> fixtureBuilder)
        {
            if (fixtureBuilder == null)
            {
                throw new ArgumentNullException(nameof(fixtureBuilder));
            }

            _fixtureLazy = new Lazy<IFixture>(fixtureBuilder, LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>
        /// Constructs a parameter from a given <see cref="IParameterInfo"/>
        /// </summary>
        /// <param name="parameter">A representation of the parameter to create</param>
        /// <returns>Exactly an instance of the given <see cref="IParameterInfo"/></returns>
        public IEnumerable GetData(IParameterInfo parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var context = new SpecimenContext(Fixture);

            var obj = context.Resolve(parameter.ParameterType);

            return new [] {obj};
        }
    }
}