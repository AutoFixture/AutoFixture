using System;

namespace Ploeh.AutoFixture.Kernel
{
    public class DirectBaseTypeSpecification : IRequestSpecification
    {
        private readonly Type targetType;

        public DirectBaseTypeSpecification(Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            this.targetType = targetType;
        }

        public Type TargetType
        {
            get { return this.targetType; }
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
                   IsDirectBaseOfTargetType(request);
        }

        private bool IsSameAsTargetType(object request)
        {
            return (Type)request == this.targetType;
        }

        private bool IsDirectBaseOfTargetType(object request)
        {
            return (Type)request == this.targetType.BaseType;
        }
    }
}
