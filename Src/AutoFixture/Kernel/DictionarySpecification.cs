using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates logic that determines whether a request is a request for a dictionary.
    /// </summary>
    public class DictionarySpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen to determine whether it's a request for a
        /// dictionary.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a request for a
        /// <see cref="IDictionary{TKey, TValue}" />; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type;
            if (type == null)
            {
                return false;
            }

            if (!type.GetConstructors().Any())
            {
                return false;
            }

            // TODO: After updating to .Net 4.5, check for IReadOnlyDictionary<,>

            var dictionaryInterfaces =
                from i in type.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                select i;

            return dictionaryInterfaces.Any();
        }
    }
}
