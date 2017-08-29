using System;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// Represents an error during the dynamic proxy creation.
    /// </summary>
#if SYSTEM_RUNTIME_SERIALIZATION
    [Serializable]
#endif
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

#if SYSTEM_RUNTIME_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of the <see cref="Ploeh.SemanticComparison.ProxyCreationException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected ProxyCreationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}