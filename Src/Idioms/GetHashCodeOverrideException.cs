using System;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Represents an error about an ill-behaved override of the <see cref="Object.GetHashCode()"/>
    /// method.
    /// </summary>
    [Serializable]
    public class GetHashCodeOverrideException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHashCodeOverrideException"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GetHashCode", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        public GetHashCodeOverrideException()
            : base("The Object.GetHashCode() override is ill-behaved.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHashCodeOverrideException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public GetHashCodeOverrideException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHashCodeOverrideException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public GetHashCodeOverrideException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHashCodeOverrideException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected GetHashCodeOverrideException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}