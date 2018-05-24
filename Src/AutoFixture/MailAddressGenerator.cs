#if SYSTEM_NET_MAIL

using System;
using System.Globalization;
using System.Net.Mail;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates new <see cref="MailAddress"/> instances.
    /// </summary>
    public class MailAddressGenerator : ISpecimenBuilder
    {
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
        /// so as to avoid any possibility of tests bothering real email addresses.
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!typeof(MailAddress).Equals(request))
            {
                return new NoSpecimen();
            }

            try
            {
                return TryCreateMailAddress(context);
            }
            catch (FormatException)
            {
                return new NoSpecimen();
            }
        }

        private static object TryCreateMailAddress(ISpecimenContext context)
        {
            var localPart = context.Resolve(typeof(EmailAddressLocalPart)) as EmailAddressLocalPart;
            var domainName = context.Resolve(typeof(DomainName)) as DomainName;

            if (localPart == null || domainName == null)
            {
                return new NoSpecimen();
            }

            var email = string.Format(CultureInfo.InvariantCulture, "{0} <{0}@{1}>", localPart, domainName);
            return new MailAddress(email);
        }
    }
}

#endif
