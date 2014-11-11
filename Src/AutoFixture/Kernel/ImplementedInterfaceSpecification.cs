using System;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    public class ImplementedInterfaceSpecification : IRequestSpecification
    {
        private readonly Type targetType;

        public ImplementedInterfaceSpecification(Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            this.targetType = targetType;
        }

        public Type TargetType
        {
            get { return targetType; }
        }

        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return IsRequestForType(request) &&
                   RequestedTypeMatchesCriteria(request);
        }

        private bool IsRequestForType(object request)
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
