using System;

namespace Ploeh.AutoFixture.Kernel
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
    }
}
