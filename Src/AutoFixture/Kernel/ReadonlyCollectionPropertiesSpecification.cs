using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates whether or not a request is for a type containing readonly properties that
    /// implement <see cref="ICollection{T}"/>.
    /// </summary>
    public class ReadonlyCollectionPropertiesSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates whether or not the <paramref name="request"/> is for a type containing readonly properties that
        /// implement <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="request">
        /// The specimen request.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <paramref name="request"/> is for a type containing readonly properties that
        /// implement <see cref="ICollection{T}"/>; <see langword="false"/> otherwise.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (!(request is Type requestType)) return false;
            if (typeof(Expression).GetTypeInfo().IsAssignableFrom(requestType)) return false;

            return requestType.GetTypeInfo()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(pi => pi.GetSetMethod() == null
                           && pi.PropertyType.GenericTypeArguments?.Length == 1
                           && (pi.PropertyType.Name == typeof(ICollection<>).Name
                               || pi.PropertyType.GetTypeInfo().GetInterface(typeof(ICollection<>).Name) != null));
        }
    }
}