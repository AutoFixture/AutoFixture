using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates logic that determines whether a request is a request for a
    /// <see cref="SortedDictionary{TKey, TValue}"/>.
    /// </summary>
    public class SortedDictionarySpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen to determine whether it's a request for a
        /// <see cref="SortedDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a request for a
        /// <see cref="SortedDictionary{TKey, TValue}" />; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type;

            if (type == null)
            {
                return false;
            }

            return type.IsGenericType 
                && typeof(SortedDictionary<,>) == type.GetGenericTypeDefinition();
        }
    }
}
