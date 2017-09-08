using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Enables auto-mocking with Moq.
    /// </summary>
    public class AutoMoqCustomization : ICustomization
    {
        private readonly ISpecimenBuilder relay;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMoqCustomization"/> class.
        /// </summary>
        public AutoMoqCustomization()
            : this(new MockRelay())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMoqCustomization"/> class with a
        /// <see cref="MockRelay"/>.
        /// </summary>
        /// <param name="relay">The relay.</param>
        public AutoMoqCustomization(ISpecimenBuilder relay)
        {
            if (relay == null)
            {
                throw new ArgumentNullException("relay");
            }

            this.relay = relay;
        }

        /// <summary>
        /// Gets the relay that will be added to <see cref="IFixture.ResidueCollectors"/> when
        /// <see cref="Customize"/> is invoked.
        /// </summary>
        /// <seealso cref="AutoMoqCustomization(ISpecimenBuilder)"/>
        public ISpecimenBuilder Relay
        {
            get { return this.relay; }
        }

        /// <summary>
        /// Customizes a <see cref="IFixture"/> to enable auto-mocking with Moq.
        /// </summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(
                new MockPostprocessor(
                    new MethodInvoker(
                        new MockConstructorQuery())));
            fixture.ResidueCollectors.Add(this.Relay);
        }
    }
}
