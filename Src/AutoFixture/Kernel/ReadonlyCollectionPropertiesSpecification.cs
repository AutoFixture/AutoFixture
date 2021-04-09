using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates whether or not a request is for a type containing readonly properties that
    /// implement <see cref="ICollection{T}"/>.
    /// </summary>
    public class ReadonlyCollectionPropertiesSpecification : IRequestSpecification
    {
        /// <summary>
        /// The default query that will be applied to select readonly collection properties.
        /// </summary>
        public static readonly IPropertyQuery DefaultPropertyQuery = new AndPropertyQuery(
            new ReadonlyPropertyQuery(),
            new GenericCollectionPropertyQuery());

        /// <summary>
        /// Constructs an instance of <see cref="ReadonlyCollectionPropertiesSpecification"/> with a default
        /// query applied for selection of readonly collection properties.
        /// </summary>
        public ReadonlyCollectionPropertiesSpecification()
            : this(DefaultPropertyQuery)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="ReadonlyCollectionPropertiesSpecification"/>, which will use the query
        /// supplied in <paramref name="propertyQuery"/> to determine whether or not a type contains readonly collection
        /// properties.
        /// </summary>
        /// <param name="propertyQuery">The query that will be applied to select readonly collection properties.</param>
        public ReadonlyCollectionPropertiesSpecification(IPropertyQuery propertyQuery)
        {
            this.PropertyQuery = propertyQuery;
        }

        /// <summary>
        /// Gets the query used to determine whether or not a specified type has readonly collection properties.
        /// </summary>
        public IPropertyQuery PropertyQuery { get; }

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
            return request is Type requestType && this.PropertyQuery.SelectProperties(requestType).Any();
        }
    }
}