using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Represents an error about an ill-behaved implementation of the <see cref="IEqualityComparer{T}" /> interface.
    /// </summary>
    [Serializable]
    public class EqualityComparerImplementationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of teh <see cref="EqualityComparerImplementationException"/> class.
        /// </summary>
        public EqualityComparerImplementationException()
            : base("The implementation of IEqualityComparer<T> is ill-behaved.")
        {
        }

        /// <summary>
        /// Initializes a new instance of teh <see cref="EqualityComparerImplementationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public EqualityComparerImplementationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of teh <see cref="EqualityComparerImplementationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EqualityComparerImplementationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of teh <see cref="EqualityComparerImplementationException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected EqualityComparerImplementationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}