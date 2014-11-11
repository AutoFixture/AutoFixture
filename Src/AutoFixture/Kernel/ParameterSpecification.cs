using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class ParameterSpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private readonly string targetName;
        private ParameterInfo requestedParameter;

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
                   ParameterIsCompatibleWithTargetType() &&
                   ParameterHasTargetName();
        }

        private bool IsRequestForParameter(object request)
        {
            this.requestedParameter = request as ParameterInfo;
            return this.requestedParameter != null;
        }

        private bool ParameterIsCompatibleWithTargetType()
        {
            return this.requestedParameter
                       .ParameterType
                       .IsAssignableFrom(this.targetType);
        }

        private bool ParameterHasTargetName()
        {
            return this.requestedParameter.Name == this.targetName;
        }
    }
}
