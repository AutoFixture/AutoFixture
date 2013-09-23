using System;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Represents an error about an ill-behaved override of the <see cref="Object.Equals(object)"/>
    /// method.
    /// </summary>
    [Serializable]
    public class EqualsOverrideException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsOverrideException"/> class.
        /// </summary>
        public EqualsOverrideException()
            : base("The Object.Equals(object) override is ill-behaved.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsOverrideException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public EqualsOverrideException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsOverrideException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public EqualsOverrideException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsOverrideException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected EqualsOverrideException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
