using System;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Represents an error about a method that incorrectly implements the idiom tested
    /// by the <see cref="CopyAndUpdateAssertion"/>.
    /// </summary>
    [Serializable]
    public class CopyAndUpdateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        public CopyAndUpdateException()
            : base("The 'copy and update' method is ill-behaved.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public CopyAndUpdateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public CopyAndUpdateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected CopyAndUpdateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
