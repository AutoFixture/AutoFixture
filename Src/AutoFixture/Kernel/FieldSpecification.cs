using System;
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
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (targetName == null)
            {
                throw new ArgumentNullException("targetName");
            }

            this.targetType = targetType;
            this.targetName = targetName;
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
                   FieldHasTargetName(request);
        }

        private bool IsRequestForField(object request)
        {
            return request is FieldInfo;
        }

        private bool FieldIsCompatibleWithTargetType(object request)
        {
            return ((FieldInfo)request)
                   .FieldType
                   .IsAssignableFrom(this.targetType);
        }

        private bool FieldHasTargetName(object request)
        {
            return ((FieldInfo)request).Name == this.targetName;
        }
    }
}
