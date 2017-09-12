using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy2
{
    /// <summary>
    /// Enables auto-mocking with FakeItEasy.
    /// </summary>
    [Obsolete("The AutoFakeItEasy2 package has been retired; use the AutoFakeItEasy (without the trailing \"2\") package instead. Details: it's turned out that it's possible to enable AutoFakeItEasy to also work with FakeItEasy 2. From version 3.49.1, you should be able to use AutoFakeItEasy with FakeItEasy 2 by adding an assembly binding redirect. This enables us, the maintainers of AutoFixture, to maintain only one code base for FakeItEasy, instead of two. If this causes problems, please create an issue at https://github.com/AutoFixture/AutoFixture/issues. We apologise for any inconvenience this may cause.", true)]
    public class AutoFakeItEasyCustomization : ICustomization
    {
        private readonly ISpecimenBuilder relay;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFakeItEasyCustomization"/> class.
        /// </summary>
        public AutoFakeItEasyCustomization()
            : this(new FakeItEasyRelay())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFakeItEasyCustomization"/> class with a
        /// <see cref="FakeItEasyRelay"/>.
        /// </summary>
        /// <param name="relay">The relay.</param>
        public AutoFakeItEasyCustomization(ISpecimenBuilder relay)
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
        /// <seealso cref="AutoFakeItEasyCustomization(ISpecimenBuilder)"/>
        public ISpecimenBuilder Relay
        {
            get { return this.relay; }
        }

        /// <summary>
        /// Customizes an <see cref="IFixture"/> to enable auto-mocking with FakeItEasy.
        /// </summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(
                new FakeItEasyBuilder(
                    new MethodInvoker(
                        new FakeItEasyMethodQuery())));
            fixture.ResidueCollectors.Add(this.Relay);
        }
    }
}
