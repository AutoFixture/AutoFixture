using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>Enables auto-mocking with NSubstitute.</summary>
    public class AutoNSubstituteCustomization : ICustomization
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>Initializes a new instance of the <see cref="AutoNSubstituteCustomization"/> class.</summary>
        /// <remarks>Uses a new instance of <see cref="NSubstituteBuilder"/> as the builder.</remarks>
        public AutoNSubstituteCustomization()
            : this(new SubstituteRelay())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="AutoNSubstituteCustomization"/> class.</summary>
        /// <param name="builder">The builder to use to create specimens for this customization.</param>
        public AutoNSubstituteCustomization(ISpecimenBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            this.builder = builder;
        }

        /// <summary>Gets the builder that will be added to <see cref="IFixture.ResidueCollectors"/> when <see cref="Customize"/> is invoked.</summary>
        /// <seealso cref="AutoNSubstituteCustomization(ISpecimenBuilder)"/>
        public ISpecimenBuilder Builder
        {
            get { return builder; }
        }

        /// <summary>Customizes an <see cref="IFixture"/> to enable auto-mocking with NSubstitute.</summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            fixture.Customizations.Insert(0, new SubstituteRequestHandler(new MethodInvoker(new NSubstituteMethodQuery())));
            fixture.Customizations.Insert(0, new SubstituteAttributeRelay());
            fixture.ResidueCollectors.Add(Builder);
        }
    }
}
