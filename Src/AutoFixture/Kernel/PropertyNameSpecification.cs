using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class PropertyNameSpecification : IRequestSpecification
    {
        private readonly string propertyName;

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

            var requestedProperty = request as PropertyInfo;
            return requestedProperty != null &&
                   requestedProperty.Name == this.propertyName;
        }
    }
}
