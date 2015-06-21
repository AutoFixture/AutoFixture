using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a constrained string to a <see cref="ConstrainedStringRequest"/>.
    /// </summary>
    public class StringLengthAttributeRelay : AttributeRelay<StringLengthAttribute>
    {
        /// <summary>
        /// Creates a <see cref="ConstrainedStringRequest"/> based on the 
        /// <see cref="StringLengthAttribute.MaximumLength"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1",
            Justification = "All arguments are guaranteed to be non-null by the base class.")]
        protected override object CreateRelayedRequest(ICustomAttributeProvider request, StringLengthAttribute attribute)
        {
            return new ConstrainedStringRequest(attribute.MaximumLength);
        }
    }
}
