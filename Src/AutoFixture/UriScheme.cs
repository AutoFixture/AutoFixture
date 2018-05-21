using System;
using System.Text.RegularExpressions;

namespace AutoFixture
{
    /// <summary>
    /// Represents a URI scheme name. Scheme names consist of a sequence of characters beginning
    /// with a letter and followed by any combination of letters, digits, plus ('+'), period ('.'),
    /// or hyphen ('-').
    /// </summary>
    public class UriScheme : IEquatable<UriScheme>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UriScheme"/> class using "scheme" as the
        /// default URI scheme name.
        /// </summary>
        public UriScheme()
            : this("http")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UriScheme"/> class.
        /// </summary>
        /// <param name="scheme">The scheme name.</param>
        public UriScheme(string scheme)
        {
            if (scheme == null) throw new ArgumentNullException(nameof(scheme));

            if (!IsValid(scheme))
            {
                throw new ArgumentException(
                    "The provided scheme is not valid. Scheme names consist of a sequence of characters beginning with " +
                    "a letter and followed by any combination of letters, digits, plus ('+'), period ('.'), or hyphen ('-').");
            }

            this.Scheme = scheme;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the URI scheme name for this
        /// instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the URI scheme name for this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Scheme;
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
            if (obj is UriScheme other)
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
            return this.Scheme.GetHashCode();
        }

        /// <summary>
        /// Gets the scheme name.
        /// </summary>
        public string Scheme { get; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the other parameter; otherwise, false.
        /// </returns>
        public bool Equals(UriScheme other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Scheme.Equals(other.Scheme, StringComparison.Ordinal);
        }

        private static bool IsValid(string scheme)
        {
            return scheme.Length > 0 && Regex.IsMatch(scheme, "^[a-zA-Z0-9+-.]*$");
        }
    }
}
