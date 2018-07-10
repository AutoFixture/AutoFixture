using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// A set of useful helpers to simplify work with fixture customizations.
    /// </summary>
    public static class CustomizationExtensions
    {
        /// <summary>
        /// Create customization that inserts the <paramref name="builder"/> to the beginning of the
        /// customizations collection.
        /// </summary>
        public static ICustomization ToCustomization(this ISpecimenBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return new InsertCustomization(builder);
        }

        private class InsertCustomization : ICustomization
        {
            private readonly ISpecimenBuilder builder;

            public InsertCustomization(ISpecimenBuilder builder)
            {
                this.builder = builder;
            }

            public void Customize(IFixture fixture)
            {
                if (fixture == null) throw new ArgumentNullException(nameof(fixture));

                fixture.Customizations.Insert(0, this.builder);
            }
        }
    }
}