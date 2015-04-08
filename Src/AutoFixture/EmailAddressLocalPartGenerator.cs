using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates new <see cref="EmailAddressLocalPart"/> instances.
    /// </summary>
    public class EmailAddressLocalPartGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }                        

            if (request == null || !typeof(EmailAddressLocalPart).Equals(request))
            {
                return new NoSpecimen(request);
            }

            var localPart = CreateLocalPart(context);

            if (!EmailAddressLocalPart.IsValid(localPart))
                return new NoSpecimen(request);

            return new EmailAddressLocalPart(localPart);
        }
       
        /// <summary>
        /// Creates a local part using the provided Specimen Context.  If the local part is greater
        /// than  EmailAddressLocalPart.MaximumAllowableLength, truncates it to that length.  It is 
        /// still possible the localPart is invalid based on EmailAddressLocalPart's rules.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string CreateLocalPart(ISpecimenContext context)
        {
            var localPart = context.Resolve(typeof(string)) as string;

            if (localPart == null)
                return null;

            if (localPart.Length > EmailAddressLocalPart.MaximumAllowableLength)
            {
                return localPart.Substring(0, EmailAddressLocalPart.MaximumAllowableLength);
            }
            else
            {
                return localPart;
            }            
        }
    }
}
