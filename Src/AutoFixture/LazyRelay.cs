using Ploeh.AutoFixture.Kernel;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
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
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var t = request as Type;
            if (t == null || !t.IsGenericType())
                return new NoSpecimen();

            if (t.GetGenericTypeDefinition() != typeof(Lazy<>))
                return new NoSpecimen();

            var builder = (ILazyBuilder)Activator
                .CreateInstance(typeof(LazyBuilder<>)
                .MakeGenericType(t.GetTypeInfo().GetGenericArguments()));
            return builder.Create(context);
        }

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
