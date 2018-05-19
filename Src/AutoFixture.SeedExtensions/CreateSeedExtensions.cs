using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates anonymous variables from <see cref="ISpecimenContext"/> or <see cref="ISpecimenBuilder"/> instances
    /// using the passed seed instance.
    /// </summary>
    public static class CreateSeedExtensions
    {
        /// <summary>
        /// Creates an anonymous object, potentially using the supplied seed as additional
        /// information when creating the object.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="seed">
        /// Any data that adds additional information when creating the anonymous object.
        /// </param>
        /// <param name="context">The context used to resolve the type request.</param>
        /// <returns>An anonymous object.</returns>
        /// <remarks>Obsolete: Please move over to using <see cref="SpecimenFactory.Create{T}(AutoFixture.Kernel.ISpecimenContext)">Create{T}()</see> as this method will be removed in the next release</remarks>
        [Obsolete("Please move over to using Create<T>() as this method will be removed in the next release", true)]
        public static T CreateAnonymous<T>(this ISpecimenContext context, T seed)
        {
            return Create<T>(context, seed);
        }

        /// <summary>Creates many anonymous objects.</summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="builder">
        /// The builder used to resolve the type request.
        /// </param>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the
        /// algorithm creating the return value.
        /// </param>
        /// <returns>
        /// A sequence of anonymous object of type <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The CreateMany implementation always returns a new instance of
        /// <see cref="IEnumerable{T}" />. Even if IEnumerable&lt;T&gt; is
        /// Frozen by the <see cref="FixtureFreezer.Freeze(IFixture)" /> method
        /// or explicitly assigned with the
        /// <see cref="FixtureRegistrar.Inject{T}(IFixture, T)" /> method, the
        /// CreateMany method returns a new, independent instance of
        /// IEnumerable&lt;T&gt;.
        /// </para>
        /// <para>
        /// However, you can change this behavior, for example by applying the
        /// <see cref="MapCreateManyToEnumerable" /> customization.
        /// </para>
        /// </remarks>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilder builder, T seed)
        {
            return builder.CreateContext().CreateMany(seed);
        }

        private static ISpecimenContext CreateContext(this ISpecimenBuilder builder)
        {
            return new SpecimenContext(builder);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="context">The context used to resolve the type request.</param>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <returns>A sequence of anonymous object of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// <para>
        /// The CreateMany implementation always returns a new instance of
        /// <see cref="IEnumerable{T}" />. Even if IEnumerable&lt;T&gt; is
        /// Frozen by the <see cref="FixtureFreezer.Freeze(IFixture)" /> method
        /// or explicitly assigned with the
        /// <see cref="FixtureRegistrar.Inject{T}(IFixture, T)" /> method, the
        /// CreateMany method returns a new, independent instance of
        /// IEnumerable&lt;T&gt;.
        /// </para>
        /// <para>
        /// However, you can change this behavior, for example by applying the
        /// <see cref="MapCreateManyToEnumerable" /> customization.
        /// </para>
        /// </remarks>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContext context, T seed)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var rawResult = context.Resolve(new MultipleRequest(new SeededRequest(typeof(T), seed)));

            return ((IEnumerable<object>)rawResult).Cast<T>();
        }

        /// <summary>Creates many anonymous objects.</summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="builder">
        /// The builder used to resolve the type request.
        /// </param>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the
        /// algorithm creating the return value.
        /// </param>
        /// <param name="count">The number of objects to create.</param>
        /// <returns>
        /// A sequence of anonymous objects of type <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The CreateMany implementation always returns a new instance of
        /// <see cref="IEnumerable{T}" />. Even if IEnumerable&lt;T&gt; is
        /// Frozen by the <see cref="FixtureFreezer.Freeze(IFixture)" /> method
        /// or explicitly assigned with the
        /// <see cref="FixtureRegistrar.Inject{T}(IFixture, T)" /> method, the
        /// CreateMany method returns a new, independent instance of
        /// IEnumerable&lt;T&gt;.
        /// </para>
        /// <para>
        /// However, you can change this behavior, for example by applying the
        /// <see cref="MapCreateManyToEnumerable" /> customization.
        /// </para>
        /// </remarks>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilder builder, T seed, int count)
        {
            return builder.CreateContext().CreateMany(seed, count);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="context">The context used to resolve the type request.</param>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <param name="count">The number of objects to create.</param>
        /// <returns>A sequence of anonymous objects of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// <para>
        /// The CreateMany implementation always returns a new instance of
        /// <see cref="IEnumerable{T}" />. Even if IEnumerable&lt;T&gt; is
        /// Frozen by the <see cref="FixtureFreezer.Freeze(IFixture)" /> method
        /// or explicitly assigned with the
        /// <see cref="FixtureRegistrar.Inject{T}(IFixture, T)" /> method, the
        /// CreateMany method returns a new, independent instance of
        /// IEnumerable&lt;T&gt;.
        /// </para>
        /// <para>
        /// However, you can change this behavior, for example by applying the
        /// <see cref="MapCreateManyToEnumerable" /> customization.
        /// </para>
        /// </remarks>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContext context, T seed, int count)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var rawResult = context.Resolve(new FiniteSequenceRequest(new SeededRequest(typeof(T), seed), count));
            return ((IEnumerable<object>)rawResult).Cast<T>();
        }

        /// <summary>
        /// Creates an anonymous object, potentially using the supplied seed as
        /// additional information when creating the object.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="builder">
        /// The builder used to resolve the type request.
        /// </param>
        /// <param name="seed">
        /// Any data that adds additional information when creating the
        /// anonymous object.
        /// </param>
        /// <returns>An anonymous object.</returns>
        public static T Create<T>(this ISpecimenBuilder builder, T seed)
        {
            return builder.CreateContext().Create<T>(seed);
        }

        /// <summary>
        /// Creates an anonymous object, potentially using the supplied seed as additional
        /// information when creating the object.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="seed">
        /// Any data that adds additional information when creating the anonymous object.
        /// </param>
        /// <param name="context">The context used to resolve the type request.</param>
        /// <returns>An anonymous object.</returns>
        public static T Create<T>(this ISpecimenContext context, T seed)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return (T)context.Resolve(new SeededRequest(typeof(T), seed));
        }
    }
}
