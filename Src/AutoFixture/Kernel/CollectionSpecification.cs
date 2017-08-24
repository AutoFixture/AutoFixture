using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates logic that determines whether a request is a request for an
    /// <see cref="ICollection{T}"/>.
    /// </summary>
    public class CollectionSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen to determine whether it's a request for an
        /// <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a request for an
        /// <see cref="ICollection{T}" />; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type;
            if (type == null)
            {
                return false;
            }

            // Do not allow interfaces, abstract classes, or other types that cannot be constructed
            if (!type.GetConstructors().Any())
            {
                return false;
            }

            // Arrays are read-only and cannot be filled
            if (type.IsArray)
            {
                return false;
            }

            // Read-only collections cannot be filled
            if (IsReadOnlyCollection(type))
            {
                return false;
            }

            var collectionInterfaces =
                from i in type.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)
                select i;

            return collectionInterfaces.Any();
        }

        private static bool IsReadOnlyCollection(Type type)
        {
            // TODO: After updating to .Net 4.5, simply check for inheriting from interface IReadOnlyDictionary<,>

            if (type == typeof(object))
            {
                return false;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ReadOnlyCollection<>))
            {
                return true;
            }

            return IsReadOnlyCollection(type.BaseType);
        }
    }
}
