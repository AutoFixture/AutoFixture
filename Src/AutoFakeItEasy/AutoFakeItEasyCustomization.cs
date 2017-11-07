using System;
using AutoFixture.Kernel;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// Enables auto-mocking with FakeItEasy.
    /// </summary>
    public class AutoFakeItEasyCustomization : ICustomization
    {
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
            this.Relay = relay ?? throw new ArgumentNullException(nameof(relay));
        }

        /// <summary>
        /// Gets the relay that will be added to <see cref="IFixture.ResidueCollectors"/> when
        /// <see cref="Customize"/> is invoked.
        /// </summary>
        /// <seealso cref="AutoFakeItEasyCustomization(ISpecimenBuilder)"/>
        public ISpecimenBuilder Relay { get; }

        /// <summary>
        /// Customizes an <see cref="IFixture"/> to enable auto-mocking with FakeItEasy.
        /// </summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.Customizations.Add(
                new FakeItEasyBuilder(
                    new MethodInvoker(
                        new FakeItEasyMethodQuery())));
            fixture.ResidueCollectors.Add(this.Relay);
        }
    }
}
