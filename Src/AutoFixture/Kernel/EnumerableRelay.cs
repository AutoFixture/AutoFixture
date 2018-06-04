using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for <see cref="IEnumerable{T}" /> to a <see cref="MultipleRequest"/> and
    /// converts the result to a sequence of the requested type.
    /// </summary>
    public class EnumerableRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new sequence of items based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A sequence of the requested type if possible; otherwise a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for <see cref="IEnumerable{T}" /> and
        /// <paramref name="context"/> can satisfy a <see cref="MultipleRequest"/> for the item
        /// type, the return value is a populated sequence of the requested type. If not, the
        /// return value is a <see cref="NoSpecimen"/> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // This is performance-sensitive code when used repeatedly over many requests.
            // See discussion at https://github.com/AutoFixture/AutoFixture/pull/218
            var type = request as Type;
            if (type == null)
                return new NoSpecimen();

            if (!type.TryGetSingleGenericTypeArgument(typeof(IEnumerable<>), out Type enumerableType))
                return new NoSpecimen();

            var specimen = context.Resolve(new MultipleRequest(enumerableType));
            if (specimen is OmitSpecimen)
                return specimen;

            var enumerable = specimen as IEnumerable<object>;
            if (enumerable == null)
                return new NoSpecimen();

            var typedAdapterType = typeof(ConvertedEnumerable<>).MakeGenericType(enumerableType);
            return Activator.CreateInstance(typedAdapterType, enumerable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "It's activated via reflection.")]
        private class ConvertedEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<object> enumerable;

            public ConvertedEnumerable(IEnumerable<object> enumerable)
            {
                this.enumerable = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
            }

            public IEnumerator<T> GetEnumerator()
            {
                foreach (var item in this.enumerable)
                {
                    if (item is T variable)
                        yield return variable;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}
