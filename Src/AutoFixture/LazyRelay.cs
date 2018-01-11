using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Relays a request for an <see cref="Func{T}" /> to a request for a
    /// <see cref="Lazy{T}"/> and returns the result.
    /// </summary>
    public class LazyRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.
        /// </param>
        /// <param name="context">A context that can be used to create other
        /// specimens.</param>
        /// <returns>
        /// An instance of a <see cref="Lazy{T}"/> if possible; otherwise a
        /// <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for a
        /// <see cref="Lazy{T}"/> and <paramref name="context"/> can satisfy a
        /// request for a <see cref="Func{T}" /> specimen,
        /// the <see cref="Func{T}" />'s value will be used when lazy
        /// initialization occurs. If not, the return value is a
        /// <see cref="NoSpecimen"/> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var type = request as Type;
            if (type == null)
                return new NoSpecimen();

            if (!type.TryGetSingleGenericTypeArgument(typeof(Lazy<>), out Type lazyType))
                return new NoSpecimen();

            var lazyBuilderType = typeof(LazyBuilder<>).MakeGenericType(lazyType);
            var builder = (ILazyBuilder)Activator.CreateInstance(lazyBuilderType);

            return builder.Create(context);
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "It's activated via reflection.")]
        private class LazyBuilder<T> : ILazyBuilder
        {
            public object Create(ISpecimenContext context)
            {
                var f = (Func<T>)context.Resolve(typeof(Func<T>));
                return new Lazy<T>(f);
            }
        }

        private interface ILazyBuilder
        {
            object Create(ISpecimenContext context);
        }
    }
}
