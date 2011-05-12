using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Indicates that an illigal request was detected.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Certain requests are considered illegal (such as <see cref="IntPtr"/>) because satisfying
    /// them can crash the process.
    /// </para>
    /// </remarks>
    /// <seealso cref="IntPtrGuard"/>
    [Serializable]
    public class IllegalRequestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalRequestException"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IntPtr", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
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

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalRequestException"/> class.
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
        protected IllegalRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
