using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Builds, or partakes in building, anonymous variables (specimens).
    /// </summary>
    public interface ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="request"/> can be any object, but will often be a
        /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
        /// </para>
        /// <para>
        /// Note to implementers: Implementations are expected to return a
        /// <see cref="NoSpecimen"/> instance if they can't satisfy the request.
        /// </para>
        /// </remarks>
        object Create(object request, ISpecimenContext context);
    }
}
