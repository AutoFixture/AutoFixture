using System;
using AutoFixture.Kernel;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// Enables auto-mocking with FakeItEasy.
    /// </summary>
    /// <remarks>
    /// NOTICE! You can assign the customization properties to tweak the features you would like to enable. See example.
    /// <br />
    /// <code>new AutoFakeItEasyCustomization { GenerateDelegates = true }</code>
    /// </remarks>
    public class AutoFakeItEasyCustomization : ICustomization
    {
        private ISpecimenBuilder relay;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFakeItEasyCustomization"/> class.
        /// <para>
        /// NOTICE! You can assign the customization properties to tweak the features you would like to enable. Example:
        /// <br />
        /// <code>new AutoFakeItEasyCustomization { GenerateDelegates = true }</code>
        /// </para>
        /// </summary>
        public AutoFakeItEasyCustomization()
#pragma warning disable CS0618 // Type or member is obsolete
            : this(new FakeItEasyRelay())
#pragma warning restore CS0618 // Type or member is obsolete
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFakeItEasyCustomization"/> class, using the specified
        /// <paramref name="relay"/> instead of a <see cref="FakeItEasyRelay"/>.
        /// </summary>
        /// <param name="relay">The relay.</param>
        [Obsolete("This constructor is obsolete and will be removed in the future version of the product. " +
                  "Please use the AutoFakeItEasyCustomization() overload (without arguments) instead and set the Relay property.")]
        public AutoFakeItEasyCustomization(ISpecimenBuilder relay)
        {
            this.Relay = relay ?? throw new ArgumentNullException(nameof(relay));
        }

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
        /// When <c>true</c>, configures the fixture to automatically generate Fakes when a delegate is requested.
        /// </summary>
        public bool GenerateDelegates { get; set; }

        /// <summary>
        /// Specifies whether members of a Fake will be automatically setup to retrieve the return values from a fixture.
        /// </summary>
        public bool ConfigureMembers { get; set; }

        /// <summary>
        /// Customizes an <see cref="IFixture"/> to enable auto-mocking with FakeItEasy.
        /// </summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            if (this.GenerateDelegates)
            {
                fixture.Customizations.Add(new FakeItEasyRelay(new DelegateSpecification()));
            }

            ISpecimenBuilder fakeBuilder = new FakeItEasyBuilder(new MethodInvoker(new FakeItEasyMethodQuery()));

            if (this.ConfigureMembers)
            {
                fakeBuilder = new Postprocessor(
                    builder: fakeBuilder,
                    command: new CompositeSpecimenCommand(
                        new ConfigureSealedMembersCommand(),
                        new ConfigureFakeMembersCommand()));
            }

            fixture.Customizations.Add(fakeBuilder);
            fixture.ResidueCollectors.Add(this.Relay);
        }
    }
}
