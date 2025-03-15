using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for an <see cref="IAsyncEnumerator{T}"/> to
    /// an <see cref="IAsyncEnumerable{T}"/> and converts the result
    /// to an enumerator of a sequence of the requested type.
    /// </summary>
    public class AsyncEnumeratorRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates an async enumerator based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// An enumerator of the requested type if possible;
        /// otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for an <see cref="IAsyncEnumerator{T}"/> and
        /// <paramref name="context"/> can satisfy an <see cref="IAsyncEnumerable{T}"/>,
        /// the return value is an enumerator of the requested type. If not, the return value
        /// is a <see cref="NoSpecimen"/> instance.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public object Create(object request, ISpecimenContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            if (request is not Type type) return new NoSpecimen();

            if (!type.TryGetSingleGenericTypeArgument(typeof(IAsyncEnumerator<>), out Type enumeratorType))
                return new NoSpecimen();

            var specimenBuilder = (IAsyncEnumeratorBuilder)Activator.CreateInstance(
                typeof(GenericAsyncEnumeratorRelay<>).MakeGenericType(enumeratorType));

            return specimenBuilder.Create(context);
        }

        private interface IAsyncEnumeratorBuilder
        {
            object Create(ISpecimenContext context);
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "It's activated via reflection.")]
        private class GenericAsyncEnumeratorRelay<T> : IAsyncEnumeratorBuilder
        {
            public object Create(ISpecimenContext context)
            {
                var result = context.Resolve(typeof(IAsyncEnumerable<T>));

                if (result is IAsyncEnumerable<T> enumerable)
                    return enumerable.GetAsyncEnumerator();

                return new NoSpecimen();
            }
        }
    }
}
