using System;
using AutoFixture.Kernel;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Enables auto-mocking and auto-setup with Moq.
    /// Members of a mock will be automatically setup to retrieve the return values from a fixture.
    /// </summary>
    [Obsolete("This customization is obsolete and will be removed in the future versions of product. " +
              "Please use 'new AutoMoqCustomization { ConfigureMembers = true }' customization instead.")]
    public class AutoConfiguredMoqCustomization : AutoMoqCustomization
    {
        /// <summary>
        /// Creates a new instance of <see cref="AutoConfiguredMoqCustomization"/>.
        /// </summary>
        public AutoConfiguredMoqCustomization()
        {
            this.ConfigureMembers = true;
        }

        /// <summary>
        /// Creates a new instance of <see cref="AutoConfiguredMoqCustomization"/>.
        /// </summary>
        /// <param name="relay">A mock relay to be added to <see cref="IFixture.ResidueCollectors"/></param>
        public AutoConfiguredMoqCustomization(ISpecimenBuilder relay)
            : base(relay)
        {
            this.ConfigureMembers = true;
        }
    }
}
