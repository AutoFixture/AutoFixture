using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class FieldNameSpecification : IRequestSpecification
    {
        private readonly string fieldName;

        public FieldNameSpecification(string fieldName)
        {
            Require.IsNotNull(fieldName, "fieldName");
            this.fieldName = fieldName;
        }

        public string FieldName
        {
            get { return fieldName; }
        }

        public bool IsSatisfiedBy(object request)
        {
            Require.IsNotNull(request, "request");

            return IsRequestForField(request) &&
                   FieldHasSpecifiedName(request);
        }

        private static bool IsRequestForField(object request)
        {
            return request is FieldInfo;
        }

        private bool FieldHasSpecifiedName(object request)
        {
            return ((FieldInfo)request).Name == this.fieldName;
        }
    }
}
