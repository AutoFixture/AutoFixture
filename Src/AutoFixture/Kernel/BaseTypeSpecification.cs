using System;

namespace Ploeh.AutoFixture.Kernel
{
    public class BaseTypeSpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private Type requestedType;

        public BaseTypeSpecification(Type targetType)
        {
            Require.IsNotNull(targetType, "targetType");

            this.targetType = targetType;
        }

        public Type TargetType
        {
            get { return this.targetType; }
        }

        public bool IsSatisfiedBy(object request)
        {
            Require.IsNotNull(request, "request");

            this.requestedType = request as Type;
            return IsRequestForBaseType();
        }

        private bool IsRequestForBaseType()
        {
            return IsRequestForType() && RequestedTypeIsContravariantWithTargetType();
        }

        private bool IsRequestForType()
        {
            return requestedType != null;
        }

        private bool RequestedTypeIsContravariantWithTargetType()
        {
            return requestedType.IsAssignableFrom(targetType);
        }
    }
}
