using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a string that matches a regular expression to a <see cref="RegularExpressionRequest"/>.
    /// </summary>
    public class RegularExpressionAttributeRelay : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder relayBuilder = new AttributeRelay<RegularExpressionAttribute>(CreateRelayedRequest);

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
            return this.relayBuilder.Create(request, context);
        }

        private static object CreateRelayedRequest(ICustomAttributeProvider request, RegularExpressionAttribute attribute)
        {
            return new RegularExpressionRequest(attribute.Pattern);
        }
    }
}
