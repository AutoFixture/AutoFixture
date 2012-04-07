using System;
using System.Text.RegularExpressions;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Represents a URI scheme name. Scheme names consist of a sequence of characters beginning 
    /// with a letter and followed by any combination of letters, digits, plus ('+'), period ('.'),
    /// or hyphen ('-').
    /// </summary>
    public class UriScheme
    {
        private readonly string scheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriScheme"/> class using "scheme" as the
        /// default URI scheme name.
        /// </summary>
        public UriScheme()
            : this("scheme")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UriScheme"/> class.
        /// </summary>
        /// <param name="scheme">The scheme name.</param>
        public UriScheme(string scheme)
        {
            if (string.IsNullOrEmpty(scheme))
            {
                throw new ArgumentNullException("scheme");
            }

            if (!UriScheme.IsValid(scheme))
            {
                throw new ArgumentException("The provided scheme is not valid. Scheme names consist of a sequence of characters beginning with a letter and followed by any combination of letters, digits, plus ('+'), period ('.'), or hyphen ('-').");
            }

            this.scheme = scheme;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.scheme;
        }

        /// <summary>
        /// Gets the scheme name.
        /// </summary>
        public string Scheme
        {
            get { return this.scheme; }
        }

        private static bool IsValid(string scheme)
        {
            return Regex.IsMatch(scheme, "^[a-zA-Z0-9+-.]*$");
        }
    }
}
