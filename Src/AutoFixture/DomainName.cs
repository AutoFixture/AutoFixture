using System;

namespace AutoFixture
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
            this.Domain = domainName ?? throw new ArgumentNullException(nameof(domainName));
        }

        /// <summary>
        /// Get the name of the domain.
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the domain name for this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the domain name for this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Domain;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/> is equal to this instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            if (obj is DomainName other)
            {
                return this.Domain.Equals(other.Domain, StringComparison.Ordinal);
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