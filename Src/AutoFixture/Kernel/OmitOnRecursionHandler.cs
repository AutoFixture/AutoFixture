using System.Collections.Generic;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Breaks a recursion by instructing AutoFixture to ignore the current
    /// request.
    /// </summary>
    /// <seealso cref="HandleRecursiveRequest(object, IEnumerable{object})" />
    public class OmitOnRecursionHandler : IRecursionHandler
    {
        /// <summary>
        /// Handles a recursive request by instructing AutoFixture to ignore
        /// <paramref name="request" />.
        /// </summary>
        /// <param name="request">The request causing the recursion.</param>
        /// <param name="recordedRequests">
        /// Previously recorded, and yet unhandled, requests.
        /// </param>
        /// <returns>
        /// A new <see cref="OmitSpecimen" /> instance.
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
        /// This implementation instructs AutoFixture to ignore the request by
        /// returning an <see cref="OmitSpecimen" /> instance.
        /// </para>
        /// </remarks>
        public object HandleRecursiveRequest(
            object request,
            IEnumerable<object> recordedRequests)
        {
            return new OmitSpecimen();
        }
    }
}
