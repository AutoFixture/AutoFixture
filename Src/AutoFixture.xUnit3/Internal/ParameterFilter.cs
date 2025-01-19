using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit3.Internal
{
    /// <summary>
    /// Specification for identifying requests that match the test parameter by provided criteria.
    /// </summary>
    internal class ParameterFilter : IRequestSpecification
    {
        private readonly IRequestSpecification matcherSpecification;

        /// <summary>
        /// Creates an instance of type <see cref="ParameterFilter"/>.
        /// </summary>
        /// <param name="parameterInfo">The parameter.</param>
        /// <param name="flags">The flags specifying the matching criteria.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterInfo"/> is null.</exception>
        public ParameterFilter(ParameterInfo parameterInfo, Matching flags)
        {
            this.ParameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));
            this.Flags = flags;
            this.matcherSpecification = new ParameterMatcherBuilder(this.ParameterInfo)
                    .SetFlags(this.Flags).Build();
        }

        /// <summary>
        /// Gets the source parameter.
        /// </summary>
        public ParameterInfo ParameterInfo { get; }

        /// <summary>
        /// Gets the matching flags.
        /// </summary>
        public Matching Flags { get; }

        /// <inheritdoc />
        public bool IsSatisfiedBy(object request)
        {
            return this.matcherSpecification.IsSatisfiedBy(request);
        }
    }
}