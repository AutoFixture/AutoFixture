using System;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Unwraps a request for many instances and returns the results as a true dynamic sequence.
    /// </summary>
    /// <seealso cref="StableFiniteSequenceRelay" />
    public class FiniteSequenceRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates many specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// Many specimens if <paramref name="request"/> is a <see cref="FiniteSequenceRequest"/>
        /// instance; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The sequence returned is a true generator, so successive iterations will yield
        /// different sets of specimens. If this is not the desired behavior,
        /// <see cref="StableFiniteSequenceRelay" /> provides an alternative.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var manyRequest = request as FiniteSequenceRequest;
            if (manyRequest == null)
            {
                return new NoSpecimen();
            }

            return from req in manyRequest.CreateRequests()
                   let res = context.Resolve(req)
                   where !(res is OmitSpecimen)
                   select res;
        }
    }
}
