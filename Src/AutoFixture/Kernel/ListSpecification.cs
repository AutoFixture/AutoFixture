using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates logic that determines whether a request is a request for a
    /// <see cref="List{T}"/>.
    /// </summary>
    public class ListSpecification : IRequestSpecification
    {
        #region IRequestSpecification Members

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

            return type.IsGenericType
                && typeof(List<>) == type.GetGenericTypeDefinition();
        }

        #endregion
    }
}
