using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// A customization that injects a specific value of a specific <see cref="Type"/>
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
    public class InjectOnMatchCustomization : ICustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InjectOnMatchCustomization"/> class.
        /// </summary>
        /// <param name="valueToInject">The value to inject.</param>
        /// <param name="request">The request used to create a specimen to freeze.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueToInject"/> or <paramref name="request"/> is <see langword="null"/>.
        /// </exception>
        public InjectOnMatchCustomization(object valueToInject, object request)
            : this(valueToInject, request, new EqualRequestSpecification(request))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectOnMatchCustomization"/> class.
        /// </summary>
        /// <param name="valueToInject">The value to inject.</param>
        /// <param name="request">The request used to create a specimen to freeze.</param>
        /// <param name="matcher">
        /// The <see cref="IRequestSpecification"/> used to match the requests
        /// that will be satisfied by the frozen specimen.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueToInject"/>, <paramref name="request"/> or <paramref name="matcher"/> is <see langword="null"/>.
        /// </exception>
        public InjectOnMatchCustomization(object valueToInject, object request, IRequestSpecification matcher)
        {
            this.ValueToInject = valueToInject ?? throw new ArgumentNullException(nameof(valueToInject));
            this.Request = request ?? throw new ArgumentNullException(nameof(request));
            this.Matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
        }

        /// <summary>
        /// The value to inject.
        /// </summary>
        public object ValueToInject { get; }

        /// <summary>
        /// The request used to resolve specimens. By default that is TargetType.
        /// </summary>
        public object Request { get; }

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
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            this.InjectSpecimenForMatchingRequests(fixture);
        }

        private void InjectSpecimenForMatchingRequests(IFixture fixture)
        {
            fixture.Customizations.Insert(
                0,
                new FilteringSpecimenBuilder(
                    this.InjectSpecimen(),
                    this.Matcher));
        }

        private ISpecimenBuilder InjectSpecimen()
        {
            return new FixedBuilder(this.ValueToInject);
        }
    }
}