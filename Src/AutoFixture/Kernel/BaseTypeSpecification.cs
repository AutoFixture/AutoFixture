﻿using System;

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

            return IsRequestForType(request) &&
                   RequestedTypeIsSameAsTarget() ||
                   RequestedTypeIsBaseOfTarget();
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

        private bool RequestedTypeIsBaseOfTarget()
        {
            return this.requestedType == this.targetType.BaseType;
        }
    }
}
