using System;
using AutoFixture.Kernel;

namespace AutoFixture.Dsl
{
    /// <summary>
    /// Provides statements that can be used to control how specimens are created.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    public interface IFactoryComposer<T> : ISpecimenBuilder
    {
        /// <summary>
        /// Specifies a function that defines how to create a specimen from a seed.
        /// </summary>
        /// <param name="factory">The factory used to create specimens from seeds.</param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        IPostprocessComposer<T> FromSeed(Func<T, T> factory);

        /// <summary>
        /// Specifies an <see cref="ISpecimenBuilder"/> that can create specimens of the
        /// appropriate type. Mostly for advanced scenarios.
        /// </summary>
        /// <param name="factory">
        /// An <see cref="ISpecimenBuilder"/> that can create specimens of the appropriate type.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method is mostly intended for advanced scenarios where a more loosely coupled
        /// strategy is desired to create specimens, but the advantages of the strongly typed
        /// <see cref="IPostprocessComposer{T}"/> are still desired for post-processing.
        /// </para>
        /// <para>
        /// One possible scenario is for post-processing of interfaces or abstract types where
        /// auto-mocking is desired to create the instance. This method makes it possible to supply
        /// an auto-mocking <see cref="ISpecimenBuilder"/>.
        /// </para>
        /// <para>
        /// <paramref name="factory"/> must be able to create specimens of the requested type, or
        /// exceptions may be thrown.
        /// </para>
        /// </remarks>
        IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory);

        /// <summary>
        /// Specifies that an anonymous object should be created in a particular way; often by
        /// using a constructor.
        /// </summary>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        IPostprocessComposer<T> FromFactory(Func<T> factory);

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using a single input
        /// parameter for the factory.
        /// </summary>
        /// <typeparam name="TInput">
        /// The type of input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes a single constructor argument of type <typeparamref name="TInput"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory);

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using two input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes two constructor arguments of type <typeparamref name="TInput1"/> and
        /// <typeparamref name="TInput2"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory);

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using three input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes three constructor arguments of type <typeparamref name="TInput1"/>,
        /// <typeparamref name="TInput2"/> and <typeparamref name="TInput3"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory);

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using four input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput4">
        /// The type of the fourth input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes three constructor arguments of type <typeparamref name="TInput1"/>,
        /// <typeparamref name="TInput2"/>, <typeparamref name="TInput3"/> and
        /// <typeparamref name="TInput4"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory);
    }
}