using System.Collections.Generic;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Handles a recursive request.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When AutoFixture is asked to create instances of complex custom types,
    /// it uses Reflection to figure out which constructor arguments and
    /// property values to assign. If the custom types contains circular
    /// references, this will cause an infinite recursion. To prevent that,
    /// <see cref="RecursionGuard" /> monitors requests in order to detect this
    /// type of situation. If RecursionGuard detects this situation, it calls
    /// <see cref="HandleRecursiveRequest" /> on a composed IRecursionHandler.
    /// </para>
    /// </remarks>
    public interface IRecursionHandler
    {
        /// <summary>
        /// Handles a recursive request.
        /// </summary>
        /// <param name="request">The request causing the recursion.</param>
        /// <param name="recordedRequests">
        /// Previously recorded, and yet unhandled, requests.
        /// </param>
        /// <returns>An object intended to break the recursion.</returns>
        /// <remarks>
        /// <para>
        /// This method is called when AutoFixture detects an infinite
        /// recursion. The <paramref name="request" /> is the request that
        /// triggered the recursion detection;
        /// <paramref name="recordedRequests" /> contains all previous, but
        /// still unhandled requests: the current call stack, if you will.
        /// </para>
        /// </remarks>
        object HandleRecursiveRequest(object request, IEnumerable<object> recordedRequests);
    }
}
