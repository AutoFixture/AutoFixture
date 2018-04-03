using System;
using System.Runtime.Serialization;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Represents an error about a null or empty string.
    /// </summary>
    [Serializable]
    public class ArgumentNullOrEmptyException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullOrEmptyException"/> class.
        /// </summary>
        /// <param name="name">
        /// The parameter passed in to the exception.
        /// </param>
        public ArgumentNullOrEmptyException(string name)
            : base("Value should not be null or empty.", name)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullOrEmptyException"/> class.
        /// </summary>
        public ArgumentNullOrEmptyException()
            : base("An invariant was not correctly protected. Are you missing a Guard Clause?")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullOrEmptyException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ArgumentNullOrEmptyException(string message, Exception innerException)
            : base(message, innerException)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullOrEmptyException"/> class.
        /// </summary>
        /// <param name="serializationInfo">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="streamingContext">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains
        /// Contextual information about the source or destination.
        /// </param>
        protected ArgumentNullOrEmptyException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}