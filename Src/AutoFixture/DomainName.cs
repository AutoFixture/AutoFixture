using System;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Represents a domain name.
    /// </summary>
    public class DomainName
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainName"/> class. Throws ArgumentNullException
        /// if domainName is null. Throws ArgumentException if domainName is empty.
        /// </summary>
        public DomainName(string domainName)
        {
            if (domainName == null)
            {
                throw new ArgumentNullException(nameof(domainName));
            }
            this.Domain = domainName;
        }

        /// <summary>
        /// Get the name of the domain.
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the domain name for this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents the domain name for this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Domain;
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
            var other = obj as DomainName;

            if (other != null)
            {
                return this.Domain.Equals(other.Domain);
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
            return this.Domain.GetHashCode();
        }
    }
}