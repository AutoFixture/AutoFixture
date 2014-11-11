using System;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    public class ImplementedInterfaceSpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private Type requestedType;

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
                   RequestedTypeIsSameAsTarget() ||
                   RequestedInterfaceIsImplementedByTarget();
        }

        private bool IsRequestForType(object request)
        {
            this.requestedType = request as Type;
            return requestedType != null;
        }

        private bool RequestedTypeIsSameAsTarget()
        {
            return this.requestedType == this.targetType;
        }

        private bool RequestedInterfaceIsImplementedByTarget()
        {
            return this.targetType
                       .GetInterfaces()
                       .Contains(this.requestedType);
        }
    }
}
