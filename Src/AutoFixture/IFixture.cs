using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides anonymous object creation services.
    /// </summary>
    public interface IFixture : ISpecimenBuilderComposer
    {
        /// <summary>
        /// Gets the behaviors that are applied when <see cref="ISpecimenBuilderComposer.Compose"/>
        /// is invoked.
        /// </summary>
        IList<ISpecimenBuilderTransformation> Behaviors { get; }

        /// <summary>
        /// Gets customizations that <see cref="ISpecimenBuilderComposer.Compose"/> will take into
        /// accout.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is expected that customizations pre-empt whichever other
        /// <see cref="ISpecimenBuilder"/> is created by
        /// <see cref="ISpecimenBuilderComposer.Compose"/>.
        /// </para>
        /// </remarks>
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
        /// Gets the residue collectors that <see cref="ISpecimenBuilderComposer.Compose"/> will
        /// take into account.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is expected that residue collectors provide fallback mechanisms if no ealier
        /// <see cref="ISpecimenBuilder"/> can handle a request.
        /// </para>
        /// </remarks>
        IList<ISpecimenBuilder> ResidueCollectors { get; }

        /// <summary>
        /// Adds many objects to a list using the provided function to create each object.
        /// </summary>
        /// <typeparam name="T">The type of object that is contained in the list.</typeparam>
        /// <param name="collection">
        /// The list to which the created objects will be added.
        /// </param>
        /// <param name="creator">
        /// The function that creates each object which is subsequently added to
        /// <paramref name="collection"/>.
        /// </param>
        void AddManyTo<T>(ICollection<T> collection, Func<T> creator);

        /// <summary>
        /// Adds many anonymously created objects to a list.
        /// </summary>
        /// <typeparam name="T">The type of object that is contained in the list.</typeparam>
        /// <param name="collection">
        /// The list to which the anonymously created objects will be added.
        /// </param>
        void AddManyTo<T>(ICollection<T> collection);

        /// <summary>
        /// Adds many anonymously created objects to a list.
        /// </summary>
        /// <typeparam name="T">The type of object that is contained in the list.</typeparam>
        /// <param name="collection">
        /// The list to which the anonymously created objects will be added.
        /// </param>
        /// <param name="repeatCount">The number of objects created and added.</param>
        void AddManyTo<T>(ICollection<T> collection, int repeatCount);

        /// <summary>
        /// Customizes the creation algorithm for a single object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object for which the algorithm should be customized.
        /// </typeparam>
        /// <returns>
        /// A <see cref="ICustomizationComposer{T}"/> that can be used to customize the creation
        /// algorithm before creating the object.
        /// </returns>
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
        /// as the current instance, or a copy is unspecfied.
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
        void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilderComposer> composerTransformation);

        /// <summary>
        /// Injects a specific instance for a specific type.
        /// </summary>
        /// <typeparam name="T">
        /// The type for which <paramref name="item"/> should be injected.
        /// </typeparam>
        /// <param name="item">The item to inject.</param>
        void Inject<T>(T item);

        /// <summary>
        /// Registers a creation function for a specifc type.
        /// </summary>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        void Register<T>(Func<T> creator);

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// a single input parameter.
        /// </summary>
        /// <typeparam name="TInput">
        /// The type of the input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        void Register<TInput, T>(Func<TInput, T> creator);

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// two input parameters.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        void Register<TInput1, TInput2, T>(Func<TInput1, TInput2, T> creator);

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// three input parameters.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        void Register<TInput1, TInput2, TInput3, T>(Func<TInput1, TInput2, TInput3, T> creator);

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// four input parameters.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput4">
        /// The type of the fourth input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        void Register<TInput1, TInput2, TInput3, TInput4, T>(Func<TInput1, TInput2, TInput3, TInput4, T> creator);
    }
}
