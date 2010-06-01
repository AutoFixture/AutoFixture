using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates anonymous variables from <see cref="ISpecimenContainer"/> or
    /// <see cref="ISpecimenBuilderComposer"/> instances.
    /// </summary>
    public static class SpecimenFactory
    {
        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="container">The container used to resolve the type request.</param>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        public static T CreateAnonymous<T>(this ISpecimenContainer container)
        {
            return (T)container.Resolve(typeof(T));
        }

        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        public static T CreateAnonymous<T>(this ISpecimenBuilderComposer composer)
        {
            return (T)SpecimenFactory.CreateContainer(composer.Compose()).CreateAnonymous<T>();
        }

        /// <summary>
        /// Creates an anonymous object, potentially using the supplied seed as additional
        /// information when creating the object.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="seed">
        /// Any data that adds additional information when creating the anonymous object.
        /// </param>
        /// <param name="container">The container used to resolve the type request.</param>
        /// <returns>An anonymous object.</returns>
        public static T CreateAnonymous<T>(this ISpecimenContainer container, T seed)
        {
            return (T)container.Resolve(new SeededRequest(typeof(T), seed));
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
            return (T)SpecimenFactory.CreateContainer(composer.Compose()).Resolve(new SeededRequest(typeof(T), seed));
        }

        public static IEnumerable<T> CreateMany<T>(this ISpecimenContainer container)
        {
            return Enumerable.Empty<T>();
        }

        private static ISpecimenContainer CreateContainer(ISpecimenBuilder builder)
        {
            return new DefaultSpecimenContainer(builder);
        }
    }
}
