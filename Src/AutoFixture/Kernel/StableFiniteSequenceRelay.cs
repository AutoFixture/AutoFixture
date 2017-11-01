using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Unwraps a request for many instances and returns the results as a stable list.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In contrast to <see cref="FiniteSequenceRelay" /> this alternative implementation returns
    /// the sequence wrapped in a <see cref="List{Object}" />. This means that the iterator will
    /// yield the same instances across multiple iterations.
    /// </para>
    /// <para>
    /// By default this class is not used by <see cref="Fixture" />, but it can be used to override
    /// the dynamic enumerable behavior by adding it to <see cref="Fixture.Customizations" />.
    /// </para>
    /// </remarks>
    /// <seealso cref="FiniteSequenceRelay" />
    public class StableFiniteSequenceRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// Many specimens if <paramref name="request"/> is a <see cref="FiniteSequenceRequest"/>
        /// instance; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The sequence of specimens returned is stable which means that it can be iterated over
        /// more than once and be expected to yield the same instances every time.
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

            return (from req in manyRequest.CreateRequests()
                    let res = context.Resolve(req)
                    where !(res is OmitSpecimen)
                    select res)
                    .ToList();
        }
    }
}
