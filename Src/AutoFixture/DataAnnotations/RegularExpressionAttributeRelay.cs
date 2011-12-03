using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    public class RegularExpressionAttributeRelay : ISpecimenBuilder
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

            var regularExpressionAttribute = customAttributeProvider.GetCustomAttributes(typeof(RegularExpressionAttribute), inherit: true).Cast<RegularExpressionAttribute>().SingleOrDefault();
            if (regularExpressionAttribute == null)
            {
                return new NoSpecimen(request);
            }

            return context.Resolve(new RegularExpressionRequest(regularExpressionAttribute.Pattern));
        }
    }
}
