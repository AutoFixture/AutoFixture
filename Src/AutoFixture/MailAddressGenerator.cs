using System;
using System.Globalization;
using System.Net.Mail;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates new <see cref="MailAddress"/> instances.
    /// </summary>
    public class MailAddressGenerator : ISpecimenBuilder
    {
        private readonly string[] fictitiousDomains = 
        { 
            "example.com", 
            "example.net", 
            "example.org" 
        };

        /// <summary>
        /// Creates a new MailAddress.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>>
        /// The generated MailAddress will have one of the reserved domains,
        /// so as to avoid any possibility of tests bothering real email addresses
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(MailAddress).Equals(request))
            {
                return new NoSpecimen(request);
            }

            var name = Guid.NewGuid().ToString();
            var host = fictitiousDomains[(uint)name.GetHashCode() % 3];

            var email = string.Format(CultureInfo.InvariantCulture, "{0} <{0}@{1}>", name, host);
            return new MailAddress(email);
        }
    }
}