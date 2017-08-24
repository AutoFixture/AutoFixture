using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates logic that determines whether a request is a request for a
    /// <see cref="SortedSet{T}"/>.
    /// </summary>
    [Obsolete("Please move over to using CollectionSpecification as that class now generically handles the SortedSet functionality.")]
    public class SortedSetSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen to determine whether it's a request for a
        /// <see cref="SortedSet{T}"/>.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a request for a
        /// <see cref="SortedSet{T}" />; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type;

            if (type == null)
            {
                return false;
            }

            return type.IsGenericType && typeof(SortedSet<>) == type.GetGenericTypeDefinition();
        }
    }
}