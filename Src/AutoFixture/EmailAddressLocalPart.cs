using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Represents the local part of the email address, defined as everything up to, but not including, the @ sign.  
    /// Since EmailAddressLocalPart is used in constructing MailAddress, enforcement of rules on a valid email address
    /// is performed by <see cref="System.Net.Mail.MailAddress"/> and not EmailAddressLocalPart other than as noted. 
    /// </summary>
    public class EmailAddressLocalPart
    {
        private readonly string localPart;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressLocalPart"/> class. Throws ArgumentNullException
        /// if localPart is null.  Throws ArgumentException if localPart is empty.
        /// </summary>
        /// <param name="localPart">The local part.</param>                
        public EmailAddressLocalPart(string localPart)
        {
            if (localPart == null)
            {
                throw new ArgumentNullException(nameof(localPart));
            }

            if (localPart.Length == 0)
            {
                throw new ArgumentException("localPart cannot be empty");
            }

            this.localPart = localPart;
        }

        /// <summary>
        /// Get the local part.
        /// </summary>
        public string LocalPart
        {
            get { return this.localPart; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; 
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            var other = obj as EmailAddressLocalPart;
            if (other != null)
            {
                return this.localPart.Equals(other.localPart);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data 
        /// structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.localPart.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the local part for this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents the local part for this instance.
        /// </returns>
        public override string ToString()
        {
            return this.localPart;
        }
    }
}
