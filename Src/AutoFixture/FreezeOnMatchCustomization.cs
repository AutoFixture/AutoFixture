using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A customization that freezes specimens of a specific <see cref="Type"/>
    /// and uses them to satisfy requests that match a set of criteria.
    /// </summary>
    /// <remarks>
    /// The criteria that determine whether the frozen specimen will be used
    /// to satisfy a given request are specified at construction time
    /// as a <see cref="IRequestSpecification"/> object.
    /// Multiple criteria can be combined together as a boolean expression
    /// by composing them into a <see cref="AndRequestSpecification"/>
    /// or <see cref="OrRequestSpecification"/> object.
    /// </remarks>
    public class FreezeOnMatchCustomization : ICustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreezeOnMatchCustomization"/> class.
        /// </summary>
        /// <remarks>
        /// The constructed customization will use the frozen specimen
        /// to satisfy requests for the exact same type.
        /// This mimics the behavior of the <see cref="FreezingCustomization"/>.
        /// </remarks>
        /// <param name="targetType">The <see cref="Type"/> to freeze.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> is <see langword="null"/>.
        /// </exception>
        public FreezeOnMatchCustomization(Type targetType)
            : this(targetType, new ExactTypeSpecification(targetType))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreezeOnMatchCustomization"/> class.
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/> to freeze.</param>
        /// <param name="matcher">
        /// The <see cref="IRequestSpecification"/> used to match the requests
        /// that will be satisfied by the frozen specimen.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> or <paramref name="matcher"/> is null.
        /// </exception>
        public FreezeOnMatchCustomization(
            Type targetType,
            IRequestSpecification matcher)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            if (matcher == null)
            {
                throw new ArgumentNullException(nameof(matcher));
            }

            this.TargetType = targetType;
            this.Matcher = matcher;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreezeOnMatchCustomization"/> class.
        /// </summary>
        /// <param name="parameter">
        /// The <see cref="ParameterInfo"/> used to freeze specimens decorated with advanced
        /// attributes (e.g. <see cref="StringLengthAttribute"/>).
        /// </param>
        /// <param name="matcher">
        /// The <see cref="IRequestSpecification"/> used to match the requests
        /// that will be satisfied by the frozen specimen.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parameter"/> or <paramref name="matcher"/> is null.
        /// </exception>
        public FreezeOnMatchCustomization(
            ParameterInfo parameter,
            IRequestSpecification matcher)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (matcher == null)
            {
                throw new ArgumentNullException(nameof(matcher));
            }

            this.ParameterInfo = parameter;
            this.TargetType = parameter.ParameterType;
            this.Matcher = matcher;
        }

        /// <summary>
        /// The <see cref="ParameterInfo"/> describing the specimen to freeze.
        /// </summary>
        public ParameterInfo ParameterInfo { get; }

        /// <summary>
        /// The <see cref="Type"/> of the frozen specimen.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// The <see cref="IRequestSpecification"/> used to match the requests
        /// that will be satisfied by the frozen specimen.
        /// </summary>
        public IRequestSpecification Matcher { get; }

        /// <summary>
        /// Customizes the specified fixture.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            FreezeTypeForMatchingRequests(fixture);
        }

        private void FreezeTypeForMatchingRequests(IFixture fixture)
        {
            fixture.Customizations.Insert(
                0,
                new FilteringSpecimenBuilder(
                    FreezeTargetType(fixture),
                    this.Matcher));
        }

        private ISpecimenBuilder FreezeTargetType(IFixture fixture)
        {
            var context = new SpecimenContext(fixture);
            var specimen = context.Resolve(this.TargetType);
            return new FixedBuilder(specimen);
        }
    }
}
