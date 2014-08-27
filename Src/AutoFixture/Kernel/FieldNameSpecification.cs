using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class FieldNameSpecification : IRequestSpecification
    {
        private readonly string fieldName;
        private FieldInfo requestedField;

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
                   FieldHasSpecifiedName();
        }

        private bool IsRequestForField(object request)
        {
            this.requestedField = request as FieldInfo;
            return this.requestedField != null;
        }

        private bool FieldHasSpecifiedName()
        {
            return this.requestedField.Name == this.fieldName;
        }
    }
}
