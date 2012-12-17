using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    public class AutoNSubstituteCustomization : ICustomization
    {
        private readonly ISpecimenBuilder relay;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoNSubstituteCustomization"/> class.
        /// </summary>
        public AutoNSubstituteCustomization()
            : this(new NSubstituteRelay())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoNSubstituteCustomization"/> class with a
        /// <see cref="NSubstituteRelay"/>.
        /// </summary>
        /// <param name="relay">The relay.</param>
        public AutoNSubstituteCustomization(ISpecimenBuilder relay)
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
        /// <seealso cref="AutoNSubstituteCustomization(ISpecimenBuilder)"/>
        public ISpecimenBuilder Relay
        {
            get { return relay; }
        }

        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(
               new NSubstituteBuilder(
                   new MethodInvoker(
                       new NSubstituteConstructorQuery())));
            fixture.ResidueCollectors.Add(Relay);
        }
    }
}