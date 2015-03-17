﻿using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Enables auto-mocking and auto-setup with NSubstitute.
    /// Members of a substitute will be automatically setup to retrieve the return values from a fixture.
    /// </summary>
    public class AutoConfiguredNSubstituteCustomization : ICustomization
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoConfiguredNSubstituteCustomization"/> class.
        /// </summary>
        /// <remarks>Uses a new instance of <see cref="NSubstituteBuilder"/> as the builder.</remarks>
        public AutoConfiguredNSubstituteCustomization()
            : this(new NSubstituteBuilder(new MethodInvoker(new NSubstituteMethodQuery())))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoConfiguredNSubstituteCustomization"/> class.
        /// </summary>
        /// <param name="builder">The builder to use to create specimens for this customization.</param>
        public AutoConfiguredNSubstituteCustomization(ISpecimenBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            this.builder = builder;
        }

        /// <summary>
        /// Gets the builder that will be added to <see cref="IFixture.ResidueCollectors"/> when 
        /// <see cref="Customize"/> is invoked.
        /// </summary>
        /// <seealso cref="AutoConfiguredNSubstituteCustomization(ISpecimenBuilder)"/>
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

            fixture.ResidueCollectors.Add(new EnumeratorRelay());
            fixture.ResidueCollectors.Add(
                new Postprocessor(
                    Builder,
                    new CompositeSpecimenCommand(
                        new NSubstituteVirtualMethodsCommand(),
                        new NSubstituteSealedPropertiesCommand())));
        }
    }
}
