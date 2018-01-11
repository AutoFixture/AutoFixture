using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates logic that determines whether a request is a request for a
    /// <see cref="Collection{T}"/>.
    /// </summary>
    [Obsolete("This specification is obsolete. Use ExactTypeSpecification(typeof(Collection<>)) instead.")]
    public class CollectionSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen to determine whether it's a request for a
        /// <see cref="Collection{T}"/>.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a request for a
        /// <see cref="Collection{T}" />; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            return request is Type type &&
                   type.TryGetSingleGenericTypeArgument(typeof(Collection<>), out var _);
        }
    }
}
