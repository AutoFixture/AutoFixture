using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    public class StringLengthAttributeRelay : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                return new NoSpecimen();
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var customAttributeProvider = request as ICustomAttributeProvider;
            if (customAttributeProvider == null)
            {
                return new NoSpecimen(request);
            }

            var stringLengthAttribute = customAttributeProvider.GetCustomAttributes(typeof(StringLengthAttribute), inherit: true).Cast<StringLengthAttribute>().SingleOrDefault();
            if (stringLengthAttribute == null)
            {
                return new NoSpecimen(request);
            }

            return context.Resolve(new ConstrainedStringRequest(stringLengthAttribute.MaximumLength));
        }
    }
}
