using System;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A specification that determines whether the request is a request
    /// for a <see cref="Type"/> that implements the specified interface <see cref="Type"/>.
    /// </summary>
    public class ImplementedInterfaceSpecification : IRequestSpecification
    {
        private readonly Type targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplementedInterfaceSpecification"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The interface <see cref="Type"/> which
        /// the requested type should implement.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> is <see langword="null"/>.
        /// </exception>
        public ImplementedInterfaceSpecification(Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            this.targetType = targetType;
        }

        /// <summary>
        /// The interface <see cref="Type"/> which
        /// the requested type should implement.
        /// </summary>
        public Type TargetType
        {
            get { return targetType; }
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

            return IsRequestForType(request) &&
                   RequestedTypeMatchesCriteria(request);
        }

        private static bool IsRequestForType(object request)
        {
            return request is Type;
        }

        private bool RequestedTypeMatchesCriteria(object request)
        {
            return IsSameAsTargetType(request) ||
                   IsInterfaceImplementedByTargetType(request);
        }

        private bool IsSameAsTargetType(object request)
        {
            return (Type)request == this.targetType;
        }

        private bool IsInterfaceImplementedByTargetType(object request)
        {
            return this.targetType
                       .GetInterfaces()
                       .Contains((Type)request);
        }
    }
}
