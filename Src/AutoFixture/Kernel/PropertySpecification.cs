using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class PropertySpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private readonly string targetName;

        public PropertySpecification(Type targetType, string targetName)
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

            return IsRequestForProperty(request) &&
                   PropertyIsCompatibleWithTargetType(request) &&
                   PropertyHasTargetName(request);
        }

        private bool IsRequestForProperty(object request)
        {
            return request is PropertyInfo;
        }

        private bool PropertyIsCompatibleWithTargetType(object request)
        {
            return ((PropertyInfo)request)
                   .PropertyType
                   .IsAssignableFrom(this.targetType);
        }

        private bool PropertyHasTargetName(object request)
        {
            return ((PropertyInfo)request).Name == this.targetName;
        }
    }
}
