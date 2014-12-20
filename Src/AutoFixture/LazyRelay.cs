using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Relays a request for an <see cref="Func{T}" /> to a request for a
    /// <see cref="Lazy{T}"/> and retuns the result.
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
                throw new ArgumentNullException("context");

            var t = request as Type;
            if (t == null || !t.IsGenericType)
                return new NoSpecimen(request);

            if (t.GetGenericTypeDefinition() != typeof(Lazy<>))
                return new NoSpecimen(request);

            return typeof(LazyBuilder)
                 .GetMethod("Create")
                 .MakeGenericMethod(t.GetGenericArguments())
                 .Invoke(null, new[] { context });
        }

        private static class LazyBuilder
        {
            public static Lazy<T> Create<T>(ISpecimenContext context)
            {
                var f = (Func<T>)context.Resolve(typeof(Func<T>));
                return new Lazy<T>(f);
            }
        }
    }
}
