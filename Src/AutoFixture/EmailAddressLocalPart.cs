using System;

namespace AutoFixture
{
#if SYSTEM_NET_MAIL
    /// <summary>
    /// Represents the local part of the email address, defined as everything up to, but not including, the @ sign.
    /// Since EmailAddressLocalPart is used in constructing MailAddress, enforcement of rules on a valid email address
    /// is performed by <see cref="System.Net.Mail.MailAddress"/> and not EmailAddressLocalPart other than as noted.
    /// </summary>
#else
    /// <summary>
    /// Represents the local part of the email address, defined as everything up to, but not including, the @ sign.
    /// Since EmailAddressLocalPart is used in constructing MailAddress, enforcement of rules on a valid email address
    /// is performed by System.Net.Mail.MailAddress and not EmailAddressLocalPart other than as noted.
    /// </summary>
#endif
    public class EmailAddressLocalPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressLocalPart"/> class. Throws ArgumentNullException
        /// if localPart is null.  Throws ArgumentException if localPart is empty.
        /// </summary>
        /// <param name="localPart">The local part.</param>
        public EmailAddressLocalPart(string localPart)
        {
            if (localPart == null) throw new ArgumentNullException(nameof(localPart));
            if (localPart.Length == 0) throw new ArgumentException("Value cannot be empty", nameof(localPart));

            this.LocalPart = localPart;
        }

        /// <summary>
        /// Get the local part.
        /// </summary>
        public string LocalPart { get; }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/> is equal to this instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            if (obj is EmailAddressLocalPart other)
            {
                return this.LocalPart.Equals(other.LocalPart, StringComparison.Ordinal);
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
            return this.LocalPart.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the local part for this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the local part for this instance.
        /// </returns>
        public override string ToString()
        {
            return this.LocalPart;
        }
    }
}
