using System;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// Represents an error during the dynamic proxy creation.
    /// </summary>
    public class ProxyCreationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ploeh.SemanticComparison.ProxyCreationException"/> class.
        /// </summary>
        public ProxyCreationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ploeh.SemanticComparison.ProxyCreationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ProxyCreationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ploeh.SemanticComparison.ProxyCreationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ProxyCreationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}