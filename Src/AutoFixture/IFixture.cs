using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides anonymous object creation services.
    /// </summary>
    public interface IFixture : ISpecimenBuilder
    {
        /// <summary>
        /// Gets the behaviors that are applied as Decorators around other
        /// parts of a Fixture.
        /// is invoked.
        /// </summary>
        IList<ISpecimenBuilderTransformation> Behaviors { get; }

        /// <summary>
        /// Gets customizations.
        /// </summary>
        IList<ISpecimenBuilder> Customizations { get; }

        /// <summary>
        /// Gets or sets if writable properties should generally be assigned a value when 
        /// generating an anonymous object.
        /// </summary>
        bool OmitAutoProperties { get; set; }

        /// <summary>
        /// Gets or sets a number that controls how many objects are created when a
        /// <see cref="Fixture"/> creates more than one anonymous objects.
        /// </summary>
        int RepeatCount { get; set; }

        /// <summary>
        /// Gets the residue collectors.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is expected that residue collectors provide fallback mechanisms if no earlier
        /// <see cref="ISpecimenBuilder"/> can handle a request.
        /// </para>
        /// </remarks>
        IList<ISpecimenBuilder> ResidueCollectors { get; }

        /// <summary>
        /// Customizes the creation algorithm for a single object, effectively turning off all
        /// Customizations on the <see cref="IFixture"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object for which the algorithm should be customized.
        /// </typeparam>
        /// <returns>
        /// A <see cref="ICustomizationComposer{T}"/> that can be used to customize the creation
        /// algorithm before creating the object.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The Build method kicks off a Fluent API which is usually completed by invoking
        /// <see cref="SpecimenFactory.Create{T}(IPostprocessComposer{T})"/> on the method
        /// chain.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        ICustomizationComposer<T> Build<T>();

        /// <summary>
        /// Applies a customization.
        /// </summary>
        /// <param name="customization">The customization to apply.</param>
        /// <returns>An <see cref="IFixture"/> where the customization is applied.</returns>
        /// <remarks>
        /// <para>
        /// Note to implementers: the returned <see cref="IFixture"/> is expected to have
        /// <paramref name="customization"/> applied. Whether the return value is the same instance
        /// as the current instance, or a copy is unspecified.
        /// </para>
        /// </remarks>
        IFixture Customize(ICustomization customization);

        /// <summary>
        /// Customizes the creation algorithm for all objects of a given type.
        /// </summary>
        /// <typeparam name="T">The type of object to customize.</typeparam>
        /// <param name="composerTransformation">
        /// A function that customizes a given <see cref="ICustomizationComposer{T}"/> and returns
        /// the modified composer.
        /// </param>
        void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation);
    }
}
