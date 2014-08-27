using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class PropertyNameSpecification : IRequestSpecification
    {
        private readonly string propertyName;
        private PropertyInfo requestedProperty;

        public PropertyNameSpecification(string propertyName)
        {
            Require.IsNotNull(propertyName, "propertyName");

            this.propertyName = propertyName;
        }

        public string PropertyName
        {
            get { return this.propertyName; }
        }

        public bool IsSatisfiedBy(object request)
        {
            Require.IsNotNull(request, "request");

            return IsRequestForProperty(request) &&
                   PropertyHasSpecifiedName();
        }

        private bool IsRequestForProperty(object request)
        {
            this.requestedProperty = request as PropertyInfo;
            return requestedProperty != null;
        }

        private bool PropertyHasSpecifiedName()
        {
            return this.requestedProperty.Name == this.propertyName;
        }
    }
}
