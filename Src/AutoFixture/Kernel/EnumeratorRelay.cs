using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for <see cref="IEnumerator{T}" /> to
    /// <see cref="IEnumerable{T}"/> and converts the result to an enumerator
    /// of a sequence of the requested type.
    /// </summary>
    public class EnumeratorRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new enumerator based on a request.
        /// </summary>
        /// <param name="request">
        /// The request that describes what to create.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <returns>
        /// An enumerator of the requested type if possible; otherwise a
        /// <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for an
        /// <see cref="IEnumerator{T}"/> and <paramref name="context"/> can
        /// satisfy an <see cref="IEnumerable{T}"/> request for the
        /// element type, the return value is an enumerator of the
        /// requested type. If not, the return value is a
        /// <see cref="NoSpecimen"/> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var type = request as Type;
            if (type == null)
                return new NoSpecimen();

            if (!type.TryGetSingleGenericTypeArgument(typeof(IEnumerator<>), out Type enumeratorType))
                return new NoSpecimen();

            var specimenBuilder = (IEnumeratorBuilder)Activator.CreateInstance(
                typeof(GenericEnumeratorRelay<>).MakeGenericType(enumeratorType));
            return specimenBuilder.Create(context);
        }

        private interface IEnumeratorBuilder
        {
            object Create(ISpecimenContext context);
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "It's activated via reflection.")]
        private class GenericEnumeratorRelay<T> : IEnumeratorBuilder
        {
            public object Create(ISpecimenContext context)
            {
                var result = context.Resolve(typeof(IEnumerable<T>));
                if (result is IEnumerable<T> enumerable)
                    return enumerable.GetEnumerator();

                return new NoSpecimen();
            }
        }
    }
}
