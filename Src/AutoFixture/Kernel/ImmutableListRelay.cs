using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Satisfies a request for <see cref="ImmutableList{T}"/> by wrapping a <see cref="IEnumerable{T}"/>
    /// with an implementation of <see cref="ImmutableList{T}"/>.
    /// </summary>
    public class ImmutableListRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates an implementation of <see cref="ImmutableList{T}"/> wrapping a <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A finite sequence of the requested type if possible; otherwise a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (request is not Type type) return new NoSpecimen();

            if (!type.TryGetSingleGenericTypeArgument(typeof(ImmutableList<>), out Type enumerableType))
            {
                return new NoSpecimen();
            }

            var specimen = context.Resolve(new MultipleRequest(enumerableType));
            if (specimen is OmitSpecimen) return specimen;

            if (specimen is not IEnumerable<object> enumerable)
                return new NoSpecimen();

            var collection = typeof(ImmutableListRelay)
                .GetMethod(nameof(Create), BindingFlags.Static | BindingFlags.NonPublic)
                .MakeGenericMethod(enumerableType)
                .Invoke(null, new[] { enumerable });

            return collection;
        }

        private static ImmutableList<T> Create<T>(IEnumerable<object> enumerable)
        {
            return enumerable.OfType<T>().ToImmutableList();
        }
    }
}
