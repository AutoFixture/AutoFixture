using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Relays a request for a string that matches a regular expression to a <see cref="RegularExpressionRequest"/>.
    /// </summary>
    public class RegularExpressionAttributeRelay : AttributeRelay<RegularExpressionAttribute>
    {
        /// <summary>
        /// Creates a <see cref="RegularExpressionRequest"/> based on the <see cref="RegularExpressionAttribute.Pattern"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1",
            Justification = "All arguments are guaranteed to be non-null by the base class.")]
        protected override object CreateRelayedRequest(
            ICustomAttributeProvider request, 
            RegularExpressionAttribute attribute)
        {
            return new RegularExpressionRequest(attribute.Pattern);
        }
    }
}
