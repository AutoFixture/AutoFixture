﻿using System;
using NSubstitute.Core;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Enables auto-mocking and auto-setup with NSubstitute.
    /// Members of a substitute will be automatically setup to retrieve the return values from a fixture.
    /// </summary>
    public class AutoConfiguredNSubstituteCustomization : ICustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoConfiguredNSubstituteCustomization"/> class.
        /// </summary>
        /// <remarks>Uses a new instance of <see cref="NSubstituteBuilder"/> as the builder.</remarks>
        public AutoConfiguredNSubstituteCustomization()
            : this(new SubstituteRelay())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoConfiguredNSubstituteCustomization"/> class.
        /// </summary>
        /// <param name="builder">The builder to use to create specimens for this customization.</param>
        public AutoConfiguredNSubstituteCustomization(ISpecimenBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            Builder = builder;
        }

        /// <summary>
        /// Gets the builder that will be added to <see cref="IFixture.ResidueCollectors"/> when 
        /// <see cref="Customize"/> is invoked.
        /// </summary>
        /// <seealso cref="AutoConfiguredNSubstituteCustomization(ISpecimenBuilder)"/>
        public ISpecimenBuilder Builder { get; }

        /// <summary>Customizes an <see cref="IFixture"/> to enable auto-mocking with NSubstitute.</summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            var substitutionContext = SubstitutionContext.Current;

            fixture.Customizations.Insert(0, 
                new Postprocessor(
                    new SubstituteRequestHandler(new MethodInvoker(new NSubstituteMethodQuery())),
                    new CompositeSpecimenCommand(
                        new NSubstituteRegisterCallHandlerCommand(substitutionContext),
                        new NSubstituteSealedPropertiesCommand())));
            fixture.Customizations.Insert(0, new SubstituteAttributeRelay());
            fixture.ResidueCollectors.Add(Builder);
        }
    }
}
