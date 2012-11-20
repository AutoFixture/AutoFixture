using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates anonymous variables from <see cref="ISpecimenContext"/> or
    /// <see cref="ISpecimenBuilderComposer"/> instances.
    /// </summary>
    public static class SpecimenFactory
    {
        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="context">The context used to resolve the type request.</param>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static T CreateAnonymous<T>(this ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return (T)context.CreateAnonymous(default(T));
        }

        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static T CreateAnonymous<T>(this ISpecimenBuilderComposer composer)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }

            return composer.Compose().CreateContext().CreateAnonymous<T>();
        }

        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="builder">
        /// The builder used to resolve the type request.
        /// </param>
        /// <returns>
        /// An anonymous object of type <typeparamref name="T"/>.
        /// </returns>
        public static T Create<T>(this ISpecimenBuilder builder)
        {
            return builder.CreateContext().CreateAnonymous<T>();
        }

        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// <para>
        /// The only purpose of this explicit overload is to support type inferencing.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Supports type inferencing.")]
        public static T CreateAnonymous<T>(this IPostprocessComposer<T> composer)
        {
            return ((ISpecimenBuilderComposer)composer).CreateAnonymous<T>();
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
        public static T CreateAnonymous<T>(this ISpecimenContext context, T seed)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return (T)context.Resolve(new SeededRequest(typeof(T), seed));
        }

        /// <summary>
        /// Creates an anonymous object, potentially using the supplied seed as additional
        /// information when creating the object.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="seed">
        /// Any data that adds additional information when creating the anonymous object.
        /// </param>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <returns>An anonymous object.</returns>
        public static T CreateAnonymous<T>(this ISpecimenBuilderComposer composer, T seed)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }

            return composer.Compose().CreateContext().CreateAnonymous(seed);
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
            return builder.CreateContext().CreateAnonymous<T>(seed);
        }

        public static IEnumerable<T> CreateMany<T>(
            this ISpecimenBuilder builder)
        {
            return builder.CreateContext().CreateMany<T>();
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="context">The context used to resolve the type request.</param>
        /// <returns>A sequence of anonymous object of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContext context)
        {
            return context.CreateMany(default(T));
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <returns>A sequence of anonymous object of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilderComposer composer)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }

            return composer.Compose().CreateContext().CreateMany<T>();
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
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
        /// The only purpose of this explicit overload is to support type inferencing.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Supports type inferencing.")]
        public static IEnumerable<T> CreateMany<T>(this IPostprocessComposer<T> composer)
        {
            return ((ISpecimenBuilderComposer)composer).CreateMany<T>();
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
        /// </remarks>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContext context, T seed)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return from s in (IEnumerable<object>)context.Resolve(new MultipleRequest(new SeededRequest(typeof(T), seed)))
                   select (T)s;
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
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
        /// </remarks>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilderComposer composer, T seed)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }

            return composer.Compose().CreateContext().CreateMany(seed);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="context">The context used to resolve the type request.</param>
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
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContext context, int count)
        {
            return context.CreateMany(default(T), count);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
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
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilderComposer composer, int count)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }

            return composer.Compose().CreateContext().CreateMany<T>(count);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
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
        /// The only purpose of this explicit overload is to support type inferencing.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Supports type inferencing.")]
        public static IEnumerable<T> CreateMany<T>(this IPostprocessComposer<T> composer, int count)
        {
            return ((ISpecimenBuilderComposer)composer).CreateMany<T>(count);
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
        /// </remarks>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContext context, T seed, int count)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return from s in (IEnumerable<object>)context.Resolve(new FiniteSequenceRequest(new SeededRequest(typeof(T), seed), count))
                   select (T)s;
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
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
        /// </remarks>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilderComposer composer, T seed, int count)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }

            return composer.Compose().CreateContext().CreateMany(seed, count);
        }

        internal static object CreateAnonymous(this ISpecimenBuilderComposer composer, Type type)
        {
            return composer.Compose().CreateContext().Resolve(type);
        }

        private static ISpecimenContext CreateContext(this ISpecimenBuilder builder)
        {
            return new SpecimenContext(builder);
        }
    }
}
