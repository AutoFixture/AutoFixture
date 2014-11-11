using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class PropertySpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private readonly string targetName;
        private PropertyInfo requestedProperty;

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
                   PropertyIsCompatibleWithTargetType() &&
                   PropertyHasTargetName();
        }

        private bool IsRequestForProperty(object request)
        {
            this.requestedProperty = request as PropertyInfo;
            return requestedProperty != null;
        }

        private bool PropertyIsCompatibleWithTargetType()
        {
            return this.requestedProperty
                       .PropertyType
                       .IsAssignableFrom(this.targetType);
        }

        private bool PropertyHasTargetName()
        {
            return this.requestedProperty.Name == this.targetName;
        }
    }
}
