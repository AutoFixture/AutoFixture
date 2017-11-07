using System;
using AutoFixture.Kernel;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Enables auto-mocking and auto-setup with Moq.
    /// Members of a mock will be automatically setup to retrieve the return values from a fixture.
    /// </summary>
    public class AutoConfiguredMoqCustomization : ICustomization
    {
        /// <summary>
        /// Creates a new instance of <see cref="AutoConfiguredMoqCustomization"/>.
        /// </summary>
        public AutoConfiguredMoqCustomization()
            : this(new MockRelay())
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="AutoConfiguredMoqCustomization"/>.
        /// </summary>
        /// <param name="relay">A mock relay to be added to <see cref="IFixture.ResidueCollectors"/></param>
        public AutoConfiguredMoqCustomization(ISpecimenBuilder relay)
        {
            this.Relay = relay ?? throw new ArgumentNullException(nameof(relay));
        }

        /// <summary>
        /// Gets the relay that will be added to <see cref="IFixture.ResidueCollectors"/> when <see cref="Customize"/> is invoked.
        /// </summary>
        public ISpecimenBuilder Relay { get; }

        /// <summary>
        /// Customizes a <see cref="IFixture"/> to enable auto-mocking and auto-setup with Moq.
        /// Members of a mock will be automatically setup to retrieve the return values from <paramref name="fixture"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.Customizations.Add(
                new Postprocessor(
                    builder: new MockPostprocessor(
                                new MethodInvoker(
                                    new MockConstructorQuery())),
                    command: new CompositeSpecimenCommand(
                                new StubPropertiesCommand(),
                                new MockVirtualMethodsCommand(),
                                new AutoMockPropertiesCommand())));

            fixture.ResidueCollectors.Add(this.Relay);
        }
    }
}
