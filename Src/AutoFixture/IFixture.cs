using System;
using System.Collections.Generic;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Provides object creation services.
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
        /// Gets the customizations that intercept the request handling by the core.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Any <see cref="ISpecimenBuilder"/> in this list are invoked before predefined builders
        /// giving them a chance to intercept a request and resolve it.
        /// </para>
        /// <para>
        /// <see cref="Customize{T}"/> places resulting customizations in this list.
        /// </para>
        /// </remarks>
        /// <seealso cref="Behaviors"/>
        /// <seealso cref="ResidueCollectors"/>
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
        /// <seealso cref="CollectionFiller.AddManyTo{T}(IFixture, ICollection{T})" />
        /// <seealso cref="CollectionFiller.AddManyTo{T}(IFixture, ICollection{T}, Func{T})" />
        /// <seealso cref="FixtureRepeater.Repeat{T}"/>
        int RepeatCount { get; set; }

        /// <summary>
        /// Gets the residue collectors that can be used to handle requests that neither the
        /// <see cref="Customizations"/> nor the predefined builders could handle.
        /// </summary>
        /// <remarks>
        /// <para>
        /// These <see cref="ISpecimenBuilder"/> instances will be invoked if no previous builder
        /// could resolve a request. This gives you the opportunity to define fallback strategies
        /// to deal with unresolved requests.
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
        /// <para>
        /// Note that the Build method chain is best understood as a one-off Customization. It
        /// bypasses all Customizations on the <see cref="Fixture"/> instance. Instead, it allows
        /// fine-grained control when building a specific specimen. However, in most cases, adding
        /// a convention-based <see cref="ICustomization"/> is a better, more flexible option.
        /// </para>
        /// </remarks>
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
        /// <remarks>
        /// <para>
        /// The resulting <see cref="ISpecimenBuilder"/> is added to <see cref="Customizations"/>.
        /// </para>
        /// </remarks>
        void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation);
    }
}
