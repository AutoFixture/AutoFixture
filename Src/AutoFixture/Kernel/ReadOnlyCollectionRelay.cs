﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for an <see cref="IReadOnlyCollection{T}" /> to a request for a
    /// <see cref="List{T}"/> and retuns the result.
    /// </summary>
    public class ReadOnlyCollectionRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A populated <see cref="IReadOnlyCollection{T}"/> of the appropriate item type if possible; otherwise
        /// a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for an <see cref="IReadOnlyCollection{T}"/> and
        /// <paramref name="context"/> can satisfy a request for a populated specimen of that type,
        /// this value will be returned. If not, the return value is a <see cref="NoSpecimen"/>
        /// instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var t = request as Type;
            if (t == null)
                return new NoSpecimen(request);

            var typeArguments = t.GetGenericArguments();
            if (typeArguments.Length != 1 ||
                typeof(IReadOnlyCollection<>) != t.GetGenericTypeDefinition())
                return new NoSpecimen(request);

            return context.Resolve(
                typeof(List<>).MakeGenericType(typeArguments));
        }
    }
}
