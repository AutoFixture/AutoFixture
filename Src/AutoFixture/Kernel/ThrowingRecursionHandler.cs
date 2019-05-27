using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Breaks a recursion by throwing a descriptive exception.
    /// </summary>
    /// <seealso cref="HandleRecursiveRequest(object, IEnumerable{object})" />
    public class ThrowingRecursionHandler : IRecursionHandler
    {
        /// <summary>
        /// Handles a recursive request.
        /// </summary>
        /// <param name="request">The request causing the recursion.</param>
        /// <param name="recordedRequests">
        /// Previously recorded, and yet unhandled, requests.
        /// </param>
        /// <returns>
        /// An object intended to break the recursion.
        /// </returns>
        /// <exception cref="ObjectCreationException">Always.</exception>
        /// <remarks>
        /// <para>
        /// This method is called when AutoFixture detects an infinite
        /// recursion. The <paramref name="request" /> is the request that
        /// triggered the recursion detection;
        /// <paramref name="recordedRequests" /> contains all previous, but
        /// still unhandled requests: the current call stack, if you will.
        /// </para>
        /// <para>
        /// This implementation always throws a descriptive exception,
        /// instructing the user that the requested type contains a circular
        /// reference, and that this is bad API design. The message also
        /// contains instructions on how to move on.
        /// </para>
        /// </remarks>
        public object HandleRecursiveRequest(
            object request,
            IEnumerable<object> recordedRequests)
        {
            throw new ObjectCreationExceptionWithPath(
                string.Format(
                    CultureInfo.InvariantCulture,
                    @"AutoFixture was unable to create an instance of type {0} because the traversed object graph contains a circular reference. Information about the circular path follows below. This is the correct behavior when a Fixture is equipped with a ThrowingRecursionBehavior, which is the default. This ensures that you are being made aware of circular references in your code. Your first reaction should be to redesign your API in order to get rid of all circular references. However, if this is not possible (most likely because parts or all of the API is delivered by a third party), you can replace this default behavior with a different behavior: on the Fixture instance, remove the ThrowingRecursionBehavior from Fixture.Behaviors, and instead add an instance of OmitOnRecursionBehavior:

fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
    .ForEach(b => fixture.Behaviors.Remove(b));
fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                ",
                    recordedRequests.First().GetType()),
                recordedRequests);
        }
    }
}
