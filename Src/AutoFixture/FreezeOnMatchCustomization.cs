using System;
using AutoFixture.Kernel;

namespace AutoFixture
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
        /// <param name="request">The request used to create a specimen to freeze.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="request"/> is <see langword="null"/>.
        /// </exception>
        public FreezeOnMatchCustomization(object request)
        {
            this.Request = request ?? throw new ArgumentNullException(nameof(request));
            this.Matcher = new EqualRequestSpecification(request);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreezeOnMatchCustomization"/> class.
        /// </summary>
        /// <param name="request">The request used to create a specimen to freeze.</param>
        /// <param name="matcher">
        /// The <see cref="IRequestSpecification"/> used to match the requests
        /// that will be satisfied by the frozen specimen.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="request"/> or <paramref name="matcher"/> is null.
        /// </exception>
        public FreezeOnMatchCustomization(
            object request,
            IRequestSpecification matcher)
        {
            this.Request = request ?? throw new ArgumentNullException(nameof(request));
            this.Matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
        }

        /// <summary>
        /// The <see cref="Type"/> of the frozen specimen.
        /// </summary>
        [Obsolete("Please use the Request property instead.")]
        public Type TargetType => this.Request as Type;

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

            this.FreezeSpecimenForMatchingRequests(fixture);
        }

        private void FreezeSpecimenForMatchingRequests(IFixture fixture)
        {
            fixture.Customizations.Insert(
                0,
                new FilteringSpecimenBuilder(
                    this.FreezeSpecimen(fixture),
                    this.Matcher));
        }

        private ISpecimenBuilder FreezeSpecimen(IFixture fixture)
        {
            var context = new SpecimenContext(fixture);
            var specimen = context.Resolve(this.Request);
            return new FixedBuilder(specimen);
        }
    }
}
