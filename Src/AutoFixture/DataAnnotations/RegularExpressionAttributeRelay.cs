using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a string that matches a regular expression to a <see cref="RegularExpressionRequest"/>.
    /// </summary>
    public class RegularExpressionAttributeRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
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

            var customAttributeProvider = request as MemberInfo;
            if (customAttributeProvider == null)
            {
                return new NoSpecimen();
            }

            var regularExpressionAttribute = customAttributeProvider.GetCustomAttributes(typeof(RegularExpressionAttribute), inherit: true).Cast<RegularExpressionAttribute>().SingleOrDefault();
            if (regularExpressionAttribute == null)
            {
                return new NoSpecimen();
            }

            return context.Resolve(new RegularExpressionRequest(regularExpressionAttribute.Pattern));
        }
    }
}
