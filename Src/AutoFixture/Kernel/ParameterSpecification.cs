using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class ParameterSpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private readonly string targetName;

        public ParameterSpecification(Type targetType, string targetName)
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

        public Type TargetType
        {
            get { return this.targetType; }
        }

        public string TargetName
        {
            get { return this.targetName; }
        }

        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return IsRequestForParameter(request) &&
                   ParameterIsCompatibleWithTargetType(request) &&
                   ParameterHasTargetName(request);
        }

        private bool IsRequestForParameter(object request)
        {
            return request is ParameterInfo;
        }

        private bool ParameterIsCompatibleWithTargetType(object request)
        {
            return ((ParameterInfo)request)
                   .ParameterType
                   .IsAssignableFrom(this.targetType);
        }

        private bool ParameterHasTargetName(object request)
        {
            return ((ParameterInfo)request).Name == this.targetName;
        }
    }
}
