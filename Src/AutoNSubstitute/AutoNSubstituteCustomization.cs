using System;
using AutoFixture.Kernel;
using NSubstitute.Core;

namespace AutoFixture.AutoNSubstitute
{
    /// <summary>Enables auto-mocking with NSubstitute.</summary>
    /// <remarks>
    /// NOTICE! You can assign the customization properties to tweak the features you would like to enable. Example:
    /// <br />
    /// <code>new AutoNSubstituteCustomization { ConfigureMembers = true }</code>
    /// </remarks>
    public class AutoNSubstituteCustomization : ICustomization
    {
        private ISpecimenBuilder relay;

        /// <summary>Initializes a new instance of the <see cref="AutoNSubstituteCustomization"/> class.
        /// <para>
        /// NOTICE! You can assign the customization properties to tweak the features you would like to enable. Example:
        /// <br />
        /// <code>new AutoNSubstituteCustomization { ConfigureMembers = true }</code>
        /// </para>
        /// </summary>
        public AutoNSubstituteCustomization()
#pragma warning disable 618 // Type or member is obsolete
            : this(new SubstituteRelay())
#pragma warning restore 618 // Type or member is obsolete
        {
        }

        /// <summary>Initializes a new instance of the <see cref="AutoNSubstituteCustomization"/> class.</summary>
        [Obsolete("This constructor is obsolete and will be removed in a future version of the product. " +
                  "Please use the AutoNSubstituteCustomization() overload (without arguments) instead and set the Relay property.")]
        public AutoNSubstituteCustomization(ISpecimenBuilder relay)
        {
            this.relay = relay ?? throw new ArgumentNullException(nameof(relay));
        }

        /// <summary>Gets the builder that will be added to <see cref="IFixture.ResidueCollectors"/> when <see cref="Customize"/> is invoked.</summary>
        [Obsolete("This property is obsolete - use the Relay property instead.")]
        public ISpecimenBuilder Builder => this.relay;

        /// <summary>Gets or sets the relay that will be added to <see cref="IFixture.ResidueCollectors"/> when <see cref="Customize"/> is invoked.</summary>
        public ISpecimenBuilder Relay
        {
            get => this.relay;
            set => this.relay = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Specifies whether members of a substitute will be automatically configured to retrieve the return values from a fixture.
        /// </summary>
        public bool ConfigureMembers { get; set; }

        /// <summary>Customizes an <see cref="IFixture"/> to enable auto-mocking with NSubstitute.</summary>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            ISpecimenBuilder substituteBuilder = new SubstituteRequestHandler(
                new MethodInvoker(
                    new NSubstituteMethodQuery()));

            if (this.ConfigureMembers)
            {
                substituteBuilder = new Postprocessor(
                    substituteBuilder,
                    new CompositeSpecimenCommand(
                        new NSubstituteRegisterCallHandlerCommand(SubstitutionContext.Current),
                        new NSubstituteSealedPropertiesCommand()));
            }

            fixture.Customizations.Insert(0, substituteBuilder);
            fixture.Customizations.Insert(0, new SubstituteAttributeRelay());
            fixture.ResidueCollectors.Add(this.Relay);
        }
    }
}
