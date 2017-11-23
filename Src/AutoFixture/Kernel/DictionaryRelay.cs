using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for an <see cref="IDictionary{TKey, TValue}" /> to a request for a
    /// <see cref="Dictionary{TKey, TValue}"/> and returns the result.
    /// </summary>
    [Obsolete("This relay has been deprecated, use \"new TypeRelay(typeof(IDictionary<>), typeof(Dictionary<>))\" instead.")]
    public class DictionaryRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A populated <see cref="IDictionary{TKey, TValue}"/> of the appropriate item type if
        /// possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for an
        /// <see cref="IDictionary{TKey, TValue}"/> and <paramref name="context"/> can satisfy a
        /// request for a populated specimen of that type, this value will be returned. If not, the
        /// return value is a <see cref="NoSpecimen"/> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // This is performance-sensitive code when used repeatedly over many requests.
            // See discussion at https://github.com/AutoFixture/AutoFixture/pull/218
            var type = request as Type;
            if (type == null) return new NoSpecimen();
            var typeArguments = type.GetTypeInfo().GetGenericArguments();
            if (typeArguments.Length != 2) return new NoSpecimen();
            var gtd = type.GetGenericTypeDefinition();
            if (gtd != typeof(IDictionary<,>)) return new NoSpecimen();
            return context.Resolve(typeof(Dictionary<,>).MakeGenericType(typeArguments));
        }
    }
}
