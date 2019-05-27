using System;

#if SYSTEM_RUNTIME_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Indicates that an illegal request was detected.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Certain requests are considered illegal (such as <see cref="IntPtr"/>) because satisfying
    /// them can crash the process.
    /// </para>
    /// </remarks>
    /// <seealso cref="IntPtrGuard"/>
#if SYSTEM_RUNTIME_SERIALIZATION
    [Serializable]
#endif
    public class IllegalRequestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalRequestException"/> class.
        /// </summary>
        public IllegalRequestException()
            : base("An illegal request was detected. This is most likely caused by a request for an unsafe resource (such as an IntPtr) that could crash the process.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalRequestException"/> class with an
        /// error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public IllegalRequestException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalRequestException"/> class with an
        /// error message and an inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public IllegalRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if SYSTEM_RUNTIME_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalRequestException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected IllegalRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
