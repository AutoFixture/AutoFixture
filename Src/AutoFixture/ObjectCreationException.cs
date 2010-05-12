using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// The exception that is thrown when AutoFixture is unable to create an object.
    /// </summary>
    [Serializable]
    public class ObjectCreationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCreationException"/> class with a
        /// default <see cref="Exception.Message"/>.
        /// </summary>
        public ObjectCreationException()
            : base("An instance of the requested type could not be created, most like because it has no public constructor.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCreationException"/> class with a
        /// custom <see cref="Exception.Message"/>.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ObjectCreationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCreationException"/> class with a
        /// custom <see cref="Exception.Message"/> and <see cref="Exception.InnerException"/>.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception.
        /// </param>
        public ObjectCreationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Ininitalizes a new instance of the <see cref="ObjectCreationException"/> class with
        /// serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">
        /// The contextual information about the source or destination.
        /// </param>
        protected ObjectCreationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
