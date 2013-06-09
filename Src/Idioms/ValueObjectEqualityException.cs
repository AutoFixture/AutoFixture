using System;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Represents a verification error when testing whether a equals method is properly implemented for Value Object.
    /// </summary>
    [Serializable]
    public class ValueObjectEqualityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObjectEqualityException"/> class.
        /// </summary>
        public ValueObjectEqualityException()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObjectEqualityException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ValueObjectEqualityException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObjectEqualityException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ValueObjectEqualityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObjectEqualityException"/> class with
        /// serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected ValueObjectEqualityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }    
    }
}