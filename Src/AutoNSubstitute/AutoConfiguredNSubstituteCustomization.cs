using System;
using AutoFixture.Kernel;

namespace AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Enables auto-mocking and auto-setup with NSubstitute.
    /// Members of a substitute will be automatically setup to retrieve the return values from a fixture.
    /// </summary>
    [Obsolete("This customization is obsolete and will be removed in a future versions of product. " +
              "Please use 'new AutoNSubstituteCustomization { ConfigureMembers = true }' customization instead.")]
    public class AutoConfiguredNSubstituteCustomization : AutoNSubstituteCustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoConfiguredNSubstituteCustomization"/> class.
        /// </summary>
        /// <remarks>Uses a new instance of <see cref="NSubstituteBuilder"/> as the builder.</remarks>
        public AutoConfiguredNSubstituteCustomization()
        {
            this.ConfigureMembers = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoConfiguredNSubstituteCustomization"/> class.
        /// </summary>
        /// <param name="relay">The builder to use to create specimens for this customization.</param>
        public AutoConfiguredNSubstituteCustomization(ISpecimenBuilder relay)
            : base(relay)
        {
            this.ConfigureMembers = true;
        }
    }
}