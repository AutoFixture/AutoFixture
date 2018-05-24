using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Event args about a request for a specimen.
    /// </summary>
    public class RequestTraceEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTraceEventArgs"/> class with the
        /// supplied values.
        /// </summary>
        /// <param name="request">A request for a specimen.</param>
        /// <param name="depth">
        /// The recursion depth at which <paramref name="request"/> occurred.
        /// </param>
        public RequestTraceEventArgs(object request, int depth)
        {
            this.Request = request;
            this.Depth = depth;
        }

        /// <summary>
        /// Gets the recursion depth at which <see cref="Request"/> occurred.
        /// </summary>
        public int Depth { get; }

        /// <summary>
        /// Gets the original request for a specimen.
        /// </summary>
        public object Request { get; }
    }
}
