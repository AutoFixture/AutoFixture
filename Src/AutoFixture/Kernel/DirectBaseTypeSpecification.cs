using System;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that determines whether the request is a request
    /// for a <see cref="Type"/> that directly inherits from the specified <see cref="Type"/>.
    /// </summary>
    public class DirectBaseTypeSpecification : IRequestSpecification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectBaseTypeSpecification"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The <see cref="Type"/> from which
        /// the requested type should directly inherit.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> is <see langword="null"/>.
        /// </exception>
        public DirectBaseTypeSpecification(Type targetType)
        {
            this.TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }

        /// <summary>
        /// The <see cref="Type"/> from which
        /// the requested type should directly inherit.
        /// </summary>
        public Type TargetType { get; }

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
            if (request == null) throw new ArgumentNullException(nameof(request));

            return IsRequestForType(request) &&
                   this.IsTargetTypeOrItsDirectBase(request);
        }

        private static bool IsRequestForType(object request)
        {
            return request is Type;
        }

        private bool IsTargetTypeOrItsDirectBase(object request)
        {
            return this.IsSameAsTargetType(request) ||
                   this.IsDirectBaseOfTargetType(request);
        }

        private bool IsSameAsTargetType(object request)
        {
            return (Type)request == this.TargetType;
        }

        private bool IsDirectBaseOfTargetType(object request)
        {
            return (Type)request == this.TargetType.GetTypeInfo().BaseType;
        }
    }
}
