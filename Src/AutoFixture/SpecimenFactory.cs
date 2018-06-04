using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates anonymous variables from <see cref="ISpecimenContext"/> or
    /// <see cref="ISpecimenBuilder"/> instances.
    /// </summary>
    public static class SpecimenFactory
    {
        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="context">The context used to resolve the type request.</param>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        public static T Create<T>(this ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return (T)context.Resolve(new SeededRequest(typeof(T), default(T)));
        }

        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="context">The context used to resolve the type request.</param>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        /// <remarks>Obsolete: Please move over to using <see cref="Create{T}(AutoFixture.Kernel.ISpecimenContext)">Create&lt;T&gt;()</see> as this method will be removed in the next release.</remarks>
        [Obsolete("Please move over to using Create<T>() as this method will be removed in the next release", true)]
        public static T CreateAnonymous<T>(this ISpecimenContext context)
        {
            return Create<T>(context);
        }

        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="fixture">
        /// The fixture used to resolve the type request.
        /// </param>
        /// <returns>
        /// An anonymous object of type <typeparamref name="T"/>.
        /// </returns>
        [Obsolete("For compatibility to AutoFixture version 2. This method will be removed, please move to using Create<T>()", true)]
        public static T CreateAnonymous<T>(this IFixture fixture)
        {
            return fixture.Create<T>();
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
            return builder.CreateContext().Create<T>();
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
        public static T Create<T>(this IPostprocessComposer<T> composer)
        {
            return Create<T>((ISpecimenBuilder)composer);
        }

        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        /// <remarks>Obsolete: "Please move over to using <see cref="Create{T}(AutoFixture.Kernel.ISpecimenContext)">Create{T}</see> as this method will be removed in the next release.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Supports type inferencing.")]
        [Obsolete("Please move over to using Create<T>() as this method will be removed in the next release", true)]
        public static T CreateAnonymous<T>(this IPostprocessComposer<T> composer)
        {
            return Create<T>(composer);
        }

        /// <summary>Creates many anonymous objects.</summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="builder">
        /// The builder used to resolve the type request.
        /// </param>
        /// <returns>
        /// A sequence of anonymous object of type <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The CreateMany implementation always returns a new instance of
        /// <see cref="IEnumerable{T}" />. Even if IEnumerable&lt;T&gt; is
        /// Frozen by the <see cref="FixtureFreezer.Freeze{T}(IFixture)" /> method
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
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilder builder)
        {
            return builder.CreateContext().CreateMany<T>();
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="context">The context used to resolve the type request.</param>
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
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var rawResult = context.Resolve(new MultipleRequest(new SeededRequest(typeof(T), default(T))));
            return ((IEnumerable<object>)rawResult).Cast<T>();
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
        /// Frozen by the <see cref="FixtureFreezer.Freeze{T}(IFixture)" /> method
        /// or explicitly assigned with the
        /// <see cref="FixtureRegistrar.Inject{T}(IFixture, T)" /> method, the
        /// CreateMany method returns a new, independent instance of
        /// IEnumerable&lt;T&gt;.
        /// </para>
        /// <para>
        /// However, you can change this behavior, for example by applying the
        /// <see cref="MapCreateManyToEnumerable" /> customization.
        /// </para>
        /// <para>
        /// The only purpose of this explicit overload is to support type inferencing.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Supports type inferencing.")]
        public static IEnumerable<T> CreateMany<T>(this IPostprocessComposer<T> composer)
        {
            return ((ISpecimenBuilder)composer).CreateMany<T>();
        }

        /// <summary>Creates many anonymous objects.</summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="builder">
        /// The builder used to resolve the type request.
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
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilder builder, int count)
        {
            return builder.CreateContext().CreateMany<T>(count);
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
        /// <para>
        /// However, you can change this behavior, for example by applying the
        /// <see cref="MapCreateManyToEnumerable" /> customization.
        /// </para>
        /// </remarks>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContext context, int count)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var rawResult = context.Resolve(new FiniteSequenceRequest(new SeededRequest(typeof(T), default(T)), count));
            return ((IEnumerable<object>)rawResult).Cast<T>();
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
        /// However, you can change this behavior, for example by applying the
        /// <see cref="MapCreateManyToEnumerable" /> customization.
        /// </para>
        /// <para>
        /// The only purpose of this explicit overload is to support type inferencing.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Supports type inferencing.")]
        public static IEnumerable<T> CreateMany<T>(this IPostprocessComposer<T> composer, int count)
        {
            return ((ISpecimenBuilder)composer).CreateMany<T>(count);
        }

        internal static object Create(this ISpecimenBuilder composer, Type type)
        {
            return composer.CreateContext().Resolve(type);
        }

        private static ISpecimenContext CreateContext(this ISpecimenBuilder builder)
        {
            return new SpecimenContext(builder);
        }
    }
}
