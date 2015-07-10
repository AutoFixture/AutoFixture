using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a constrained string to a <see cref="ConstrainedStringRequest"/>.
    /// </summary>
    public class StringLengthAttributeRelay : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder relayBuilder = new AttributeRelay<StringLengthAttribute>(CreateRelayedRequest);

        /// <summary>
        /// Creates a new specimen based on a specified length of characters that are allowed.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created from a <see cref="RangedNumberRequest"/> encapsulating the operand
        /// type, the minimum and the maximum of the requested number, if possible; otherwise,
        /// a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            return this.relayBuilder.Create(request, context);
        }

        private static object CreateRelayedRequest(ICustomAttributeProvider request, StringLengthAttribute attribute)
        {
            return new ConstrainedStringRequest(attribute.MaximumLength);
        }
    }
}
