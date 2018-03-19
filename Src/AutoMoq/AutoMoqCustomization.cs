using System;
using AutoFixture.Kernel;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Enables auto-mocking with Moq.
    /// </summary>
    /// <remarks>
    /// NOTICE! You can assign the customization properties to tweak the features you would like to enable. Example:
    /// <br />
    /// <code>new AutoMoqCustomization { ConfigureMembers = true }</code>
    /// </remarks>
    public class AutoMoqCustomization : ICustomization
    {
        private ISpecimenBuilder relay;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMoqCustomization"/> class.
        /// <para>
        /// NOTICE! You can assign the customization properties to tweak the features you would like to enable. Example:
        /// <br />
        /// <code>new AutoMoqCustomization { ConfigureMembers = true }</code>
        /// </para>
        /// </summary>
        public AutoMoqCustomization()
#pragma warning disable 618 // Type or member is obsolete
            : this(new MockRelay())
#pragma warning restore 618 // Type or member is obsolete
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMoqCustomization"/> class with a
        /// <see cref="MockRelay"/>.
        /// </summary>
        /// <param name="relay">The relay.</param>
        [Obsolete("This constructor is obsolete and will be removed in a future version of the product. " +
                  "Please use the AutoMoqCustomization() overload (without arguments) instead and set the Relay property.")]
        public AutoMoqCustomization(ISpecimenBuilder relay)
        {
            this.relay = relay ?? throw new ArgumentNullException(nameof(relay));
        }

        /// <summary>
        /// Specifies whether members of a mock will be automatically setup to retrieve the return values from a fixture.
        /// </summary>
        public bool ConfigureMembers { get; set; }

        /// <summary>
        /// If value is <c>true</c>, delegate requests are intercepted and created by Moq.
        /// Otherwise, if value is <c>false</c>, delegates are created by the AutoFixture kernel.
        /// </summary>
        public bool GenerateDelegates { get; set; }

        /// <summary>
        /// Gets or sets the relay that will be added to <see cref="IFixture.ResidueCollectors"/> when
        /// <see cref="Customize"/> is invoked.
        /// </summary>
        public ISpecimenBuilder Relay
        {
            get => this.relay;
            set => this.relay = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Customizes an <see cref="IFixture"/> to enable auto-mocking with Moq.
        /// </summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            ISpecimenBuilder mockBuilder = new MockPostprocessor(
                                              new MethodInvoker(
                                                 new MockConstructorQuery()));

            // If members should be automatically configured, wrap the builder with members setup postprocessor.
            if (this.ConfigureMembers)
            {
                mockBuilder = new Postprocessor(
                    builder: mockBuilder,
                    command: new CompositeSpecimenCommand(
                                new StubPropertiesCommand(),
                                new MockVirtualMethodsCommand(),
                                new AutoMockPropertiesCommand()));
            }

            fixture.Customizations.Add(mockBuilder);
            fixture.ResidueCollectors.Add(this.Relay);

            if (this.GenerateDelegates)
            {
                fixture.Customizations.Add(new MockRelay(new DelegateSpecification()));
            }
        }
    }
}
