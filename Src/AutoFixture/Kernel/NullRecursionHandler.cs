using System.Collections.Generic;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Breaks a recursion by returning <see langword="null" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Some type designs contain circular references, for example in the form
    /// of parent/child relationships. Often the circular reference design
    /// involves a mutable property where a client can assign a parent to a
    /// child, or vice versa. Such a property may accept
    /// <see langword="null" /> as input, effectively breaking the circular
    /// reference. As an example, setting a Parent property to
    /// <see langword="null" /> may indicate that the instance represents a
    /// root node.
    /// </para>
    /// </remarks>
    /// <seealso cref="HandleRecursiveRequest(object, IEnumerable{object})" />
    public class NullRecursionHandler : IRecursionHandler
    {
        /// <summary>
        /// Handles a recursive request by returning <see langword="null" />.
        /// </summary>
        /// <param name="request">The request causing the recursion.</param>
        /// <param name="recordedRequests">
        /// Previously recorded, and yet unhandled, requests.
        /// </param>
        /// <returns>
        /// <see langword="null" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method is called when AutoFixture detects an infinite
        /// recursion. The <paramref name="request" /> is the request that
        /// triggered the recursion detection;
        /// <paramref name="recordedRequests" /> contains all previous, but
        /// still unhandled requests: the current call stack, if you will.
        /// </para>
        /// <para>
        /// This implementation breaks the recursion by returning null. This is
        /// not always the appropriate response, because the property or method
        /// argument requested may contain a Guard Clause preventing null from
        /// being accepted. However, if the member in question accepts null as
        /// valid input, returning null to break the recursion may be an
        /// appropriate response.
        /// </para>
        /// </remarks>
        /// <seealso cref="NullRecursionHandler" />
        public object HandleRecursiveRequest(
            object request,
            IEnumerable<object> recordedRequests)
        {
            return null;
        }
    }
}
