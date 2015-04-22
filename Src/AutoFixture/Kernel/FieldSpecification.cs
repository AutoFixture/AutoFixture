using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A specification that determines whether the request is a request
    /// for a <see cref="FieldInfo"/> matching the specified name and <see cref="Type"/>.
    /// </summary>
    public class FieldSpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private readonly string targetName;
        private readonly IEqualityComparer<string> nameComparison;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSpecification"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The <see cref="Type"/> with which the requested
        /// <see cref="FieldInfo"/> type should be compatible.
        /// </param>
        /// <param name="targetName">
        /// The name which the requested <see cref="FieldInfo"/> name
        /// should match exactly.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> or
        /// <paramref name="targetName"/> is <see langword="null"/>.
        /// </exception>
        public FieldSpecification(Type targetType, string targetName)
            : this(targetType, targetName, StringComparer.Ordinal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSpecification"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The <see cref="Type"/> with which the requested
        /// <see cref="FieldInfo"/> type should be compatible.
        /// </param>
        /// <param name="targetName">
        /// The name which the requested <see cref="FieldInfo"/> name
        /// should match according to the specified
        /// <paramref name="nameComparison"/> criteria.
        /// </param>
        /// <param name="nameComparison">
        /// The criteria used to match the requested
        /// <see cref="FieldInfo"/> name with the specified
        /// <paramref name="targetName"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> or
        /// <paramref name="targetName"/> or
        /// <paramref name="nameComparison"/> is <see langword="null"/>.
        /// </exception>
        public FieldSpecification(
            Type targetType,
            string targetName,
            IEqualityComparer<string> nameComparison)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (targetName == null)
            {
                throw new ArgumentNullException("targetName");
            }

            if (nameComparison == null)
            {
                throw new ArgumentNullException("nameComparison");
            }

            this.targetType = targetType;
            this.targetName = targetName;
            this.nameComparison = nameComparison;
        }

        /// <summary>
        /// The <see cref="Type"/> with which the requested
        /// <see cref="FieldInfo"/> type should be compatible.
        /// </summary>
        public Type TargetType
        {
            get { return this.targetType; }
        }

        /// <summary>
        /// The name which the requested <see cref="FieldInfo"/> name
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

            return IsRequestForField(request) &&
                   FieldIsCompatibleWithTargetType(request) &&
                   FieldMatchesTargetName(request);
        }

        private static bool IsRequestForField(object request)
        {
            return request is FieldInfo;
        }

        private bool FieldIsCompatibleWithTargetType(object request)
        {
            return ((FieldInfo)request)
                   .FieldType
                   .IsAssignableFrom(this.targetType);
        }

        private bool FieldMatchesTargetName(object request)
        {
            return this.nameComparison.Equals(
                ((FieldInfo)request).Name,
                this.targetName);
        }
    }
}
