using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Represents the local part of the email address, defined as everything up to, but not including the @ sign.  
    /// The requirements enforced on the local part are defined in RFC 3696.  The regular expression for validating 
    /// the local part borrowed from Phil Haack at 
    /// http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx/
    /// </summary>
    public class EmailAddressLocalPart : IEquatable<EmailAddressLocalPart>
    {
        private static string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                               + @"([-A-Za-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)$";
        private readonly string localPart;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressLocalPart"/> class. Throws ArgumentNullException
        /// if localPart is null or empty.  Throws ArgumentException if localPart is longer than 64 characters or
        /// does not conform to the local pat requirements per RFC 3696. 
        /// </summary>
        /// <param name="localPart">The local part.</param>        
        public EmailAddressLocalPart(string localPart)
        {
            if (string.IsNullOrEmpty(localPart))
            {
                throw new ArgumentNullException(localPart);
            }

            if (!EmailAddressLocalPart.IsValid(localPart))
            {
                throw new ArgumentException("Email address local part can only contain letters, digits, and special characters !#$%&'*+/=?^_`{|}~");
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
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the other parameter; otherwise, false.
        /// </returns>
        public bool Equals(EmailAddressLocalPart other)
        {
            if (other == null)
                return false; 

            return this.localPart.Equals(other.localPart);
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
                return this.Equals(other);
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

        /// <summary>
        /// Returns true if the localPart is valid in terms of both length and conformity with allowable
        /// characters and character ordering, false otherwise. 
        /// </summary>
        /// <param name="localPart"></param>
        /// <returns></returns>
        public static bool IsValid(string localPart)
        {
            if (string.IsNullOrEmpty(localPart))
                return false;

            return localPart.Length <= MaximumAllowableLength && Regex.IsMatch(localPart, ValidEmailAddressPattern);
        }

        /// <summary>
        /// Gets the maximum length, inclusive, supported as the local part. 
        /// </summary>
        public static int MaximumAllowableLength
        {
            get { return 64; }
        }

        /// <summary>
        /// Gets the pattern used by EmailAddressLocalPart to validate local parts.  This pattern is used
        /// to validate for character content, but not for length.
        /// </summary>
        public static string ValidEmailAddressPattern
        {
            get { return pattern; }
        }
    }
}
