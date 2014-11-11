using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class FieldSpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private readonly string targetName;
        private FieldInfo requestedField;

        public FieldSpecification(Type targetType, string targetName)
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

            return IsRequestForField(request) &&
                   FieldIsCompatibleWithTargetType() &&
                   FieldHasTargetName();
        }

        private bool IsRequestForField(object request)
        {
            this.requestedField = request as FieldInfo;
            return this.requestedField != null;
        }

        private bool FieldIsCompatibleWithTargetType()
        {
            return this.requestedField
                       .FieldType
                       .IsAssignableFrom(this.targetType);
        }

        private bool FieldHasTargetName()
        {
            return this.requestedField.Name == this.targetName;
        }
    }
}
