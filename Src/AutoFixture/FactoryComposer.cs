using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Dsl;
using System.ComponentModel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides extension methods for backwards compatibility.
    /// </summary>
    public static class FactoryComposer
    {
        /// <summary>
        /// Specifies that an anonymous object should be created in a particular way; often by
        /// using a constructor.
        /// </summary>
        /// <param name="composer">An <see cref="IFactoryComposer{T}"/> instance.</param>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method is deprecated. Use <see cref="IFactoryComposer{T}.FromFactory(Func{T})"/>
        /// instead.
        /// </para>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use the FromFactory method instead of the WithConstructor method.")]
        public static IPostprocessComposer<T> WithConstructor<T>(this IFactoryComposer<T> composer, Func<T> factory)
        {
            return composer.FromFactory(factory);
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using a single input
        /// parameter for the factory.
        /// </summary>
        /// <typeparam name="T">The type of specimen to create.</typeparam>
        /// <typeparam name="TInput">
        /// The type of input parameter to use when invoking <paramref name="factory"/>
        /// .</typeparam>
        /// <param name="composer">An <see cref="IFactoryComposer{T}"/> instance.</param>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes a single constructor argument of type <typeparamref name="TInput"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method is deprecated. Use
        /// <see cref="IFactoryComposer{T}.FromFactory{TInput}(Func{TInput, T})"/> instead.
        /// </para>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use the FromFactory method instead of the WithConstructor method.")]
        public static IPostprocessComposer<T> WithConstructor<T, TInput>(this IFactoryComposer<T> composer, Func<TInput, T> factory)
        {
            return composer.FromFactory(factory);
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using two input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="T">The type of specimen to create.</typeparam>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="composer">An <see cref="IFactoryComposer{T}"/> instance.</param>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes two constructor arguments of type <typeparamref name="TInput1"/> and
        /// <typeparamref name="TInput2"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method is deprecated. Use
        /// <see cref="IFactoryComposer{T}.FromFactory{TInput1, TInput2}(Func{TInput1, TInput2, T})"/>
        /// instead.
        /// </para>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use the FromFactory method instead of the WithConstructor method.")]
        public static IPostprocessComposer<T> WithConstructor<T, TInput1, TInput2>(this IFactoryComposer<T> composer, Func<TInput1, TInput2, T> factory)
        {
            return composer.FromFactory(factory);
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using three input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="T">The type of specimen to create.</typeparam>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="composer">An <see cref="IFactoryComposer{T}"/> instance.</param>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes three constructor arguments of type <typeparamref name="TInput1"/>,
        /// <typeparamref name="TInput2"/> and <typeparamref name="TInput3"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method is deprecated. Use
        /// <see cref="IFactoryComposer{T}.FromFactory{TInput1, TInput2, TInput3}(Func{TInput1, TInput2, TInput3, T})"/>
        /// instead.
        /// </para>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use the FromFactory method instead of the WithConstructor method.")]
        public static IPostprocessComposer<T> WithConstructor<T, TInput1, TInput2, TInput3>(this IFactoryComposer<T> composer, Func<TInput1, TInput2, TInput3, T> factory)
        {
            return composer.FromFactory(factory);
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using four input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="T">The type of specimen to create.</typeparam>
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
        /// <param name="composer">An <see cref="IFactoryComposer{T}"/> instance.</param>
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
        /// <remarks>
        /// <para>
        /// This method is deprecated. Use
        /// <see cref="IFactoryComposer{T}.FromFactory{TInput1, TInput2, TInput3, TInput4}(Func{TInput1, TInput2, TInput3, TInput4, T})"/>
        /// instead.
        /// </para>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use the FromFactory method instead of the WithConstructor method.")]
        public static IPostprocessComposer<T> WithConstructor<T, TInput1, TInput2, TInput3, TInput4>(this IFactoryComposer<T> composer, Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return composer.FromFactory(factory);
        }
    }
}
