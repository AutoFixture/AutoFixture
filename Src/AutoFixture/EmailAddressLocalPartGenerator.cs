using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates new <see cref="EmailAddressLocalPart"/> instances.
    /// </summary>
    public class EmailAddressLocalPartGenerator : ISpecimenBuilder
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
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (request == null || !typeof(EmailAddressLocalPart).Equals(request))
            {
                return new NoSpecimen();
            }

            var localPart = context.Resolve(typeof(string)) as string;

            if (string.IsNullOrEmpty(localPart))
            {
                return new NoSpecimen();
            }

            return new EmailAddressLocalPart(localPart);
        }
    }
}
