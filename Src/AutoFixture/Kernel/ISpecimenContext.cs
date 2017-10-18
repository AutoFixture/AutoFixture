using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A context used to create anonymous variables (specimens).
    /// </summary>
    public interface ISpecimenContext
    {
        /// <summary>
        /// Creates an anonymous variable (specimen) based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <returns>The requested specimen if possible.</returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="request"/> can be any object, but will often be a
        /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
        /// </para>
        /// </remarks>
        object Resolve(object request);
    }
}
