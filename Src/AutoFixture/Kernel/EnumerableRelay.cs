using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
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
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // This is performance-sensitive code when used repeatedly over many requests.
            // See discussion at https://github.com/AutoFixture/AutoFixture/pull/218
            var type = request as Type;
            if (type == null) return new NoSpecimen(request);
            var typeArgs = type.GetGenericArguments();
            if (typeArgs.Length != 1) return new NoSpecimen(request);
            if (type.GetGenericTypeDefinition() != typeof(IEnumerable<>)) return new NoSpecimen(request);
            var enumerable = context.Resolve(new MultipleRequest(typeArgs[0])) as IEnumerable<object>;
            if (enumerable == null) return new NoSpecimen(request);

            return typeof (ConvertedEnumerable<>)
                .MakeGenericType(typeArgs)
                .GetConstructor(new[] {typeof (IEnumerable<object>)})
                .Invoke(new[] {enumerable});
        }

        private class ConvertedEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<object> enumerable;

            public ConvertedEnumerable(IEnumerable<object> enumerable)
            {
                if (enumerable == null)
                {
                    throw new ArgumentNullException("enumerable");
                }

                this.enumerable = enumerable;
            }

            public IEnumerator<T> GetEnumerator()
            {
                foreach (var item in this.enumerable)
                    if (item is T)
                        yield return (T)item;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
