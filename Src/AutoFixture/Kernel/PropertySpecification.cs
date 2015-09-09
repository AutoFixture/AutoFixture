using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A specification that determines whether the request is a request
    /// for a <see cref="PropertyInfo"/> matching a particular property,
    /// according to the supplied comparison rules.
    /// </summary>
    public class PropertySpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private readonly string targetName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySpecification"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The <see cref="Type"/> with which the requested
        /// <see cref="PropertyInfo"/> type should be compatible.
        /// </param>
        /// <param name="targetName">
        /// The name which the requested <see cref="PropertyInfo"/> name
        /// should match exactly.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> or
        /// <paramref name="targetName"/> is <see langword="null"/>.
        /// </exception>
        public PropertySpecification(Type targetType, string targetName)
            : this(targetType, targetName, new FalseComparer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySpecification"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The <see cref="Type"/> with which the requested
        /// <see cref="PropertyInfo"/> type should be compatible.
        /// </param>
        /// <param name="targetName">
        /// The name which the requested <see cref="PropertyInfo"/> name
        /// should match according to the specified
        /// <paramref name="propertyComparer"/> criteria.
        /// </param>
        /// <param name="propertyComparer">
        /// The criteria used to match the requested
        /// <see cref="PropertyInfo"/> name with the specified
        /// <paramref name="targetName"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> or
        /// <paramref name="targetName"/> or
        /// <paramref name="propertyComparer"/> is <see langword="null"/>.
        /// </exception>
        public PropertySpecification(
            Type targetType,
            string targetName,
            IEqualityComparer<PropertyInfo> propertyComparer)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (targetName == null)
            {
                throw new ArgumentNullException("targetName");
            }

            if (propertyComparer == null)
            {
                throw new ArgumentNullException("propertyComparer");
            }

            this.targetType = targetType;
            this.targetName = targetName;
        }

        /// <summary>
        /// The <see cref="Type"/> with which the requested
        /// <see cref="PropertyInfo"/> type should be compatible.
        /// </summary>
        public Type TargetType
        {
            get { return this.targetType; }
        }

        /// <summary>
        /// The name which the requested <see cref="PropertyInfo"/> name
        /// should match exactly.
        /// </summary>
        public string TargetName
        {
            get { return this.targetName; }
        }

        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is satisfied by the Specification;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return IsRequestForProperty(request) &&
                   PropertyIsCompatibleWithTargetType(request) &&
                   PropertyMatchesTargetName(request);
        }

        private static bool IsRequestForProperty(object request)
        {
            return request is PropertyInfo;
        }

        private bool PropertyIsCompatibleWithTargetType(object request)
        {
            return ((PropertyInfo)request)
                   .PropertyType
                   .IsAssignableFrom(this.targetType);
        }

        private bool PropertyMatchesTargetName(object request)
        {
            return ((PropertyInfo)request).Name == this.targetName;
        }

        private class FalseComparer : IEqualityComparer<PropertyInfo>
        {
            public bool Equals(PropertyInfo x, PropertyInfo y)
            {
                return false;
            }

            public int GetHashCode(PropertyInfo obj)
            {
                return 0;
            }
        }
    }
}
