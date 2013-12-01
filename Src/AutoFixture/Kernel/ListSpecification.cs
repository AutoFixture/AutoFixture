using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates logic that determines whether a request is a request for a
    /// <see cref="List{T}"/>.
    /// </summary>
    public class ListSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen to determine whether it's a request for a
        /// <see cref="List{T}"/>.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a request for a
        /// <see cref="List{T}" />; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type;
            if (type == null)
            {
                return false;
            }

            if (type.IsGenericType)
            {
                var genType = type.GetGenericTypeDefinition();
                if (genType.GetHashCode() != typeof (List<>).GetHashCode())
                    return false;

                return object.Equals(genType, typeof (List<>));
            }

            return false;
        }
    }
}
