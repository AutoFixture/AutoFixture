using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class FieldSpecification : IRequestSpecification
    {
        private readonly Type targetType;
        private readonly string targetName;

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
                   FieldIsCompatibleWithTargetType(request) &&
                   FieldHasTargetName(request);
        }

        private bool IsRequestForField(object request)
        {
            return request is FieldInfo;
        }

        private bool FieldIsCompatibleWithTargetType(object request)
        {
            return ((FieldInfo)request)
                   .FieldType
                   .IsAssignableFrom(this.targetType);
        }

        private bool FieldHasTargetName(object request)
        {
            return ((FieldInfo)request).Name == this.targetName;
        }
    }
}
