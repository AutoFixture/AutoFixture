using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Unwraps a request for a string <see cref="SeededRequest"/> to a request for a string and
    /// prefixes the seed to the result.
    /// </summary>
    public class StringSeedRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates an anonymous string based on a seed.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A string with the seed prefixed to a string created by <paramref name="context"/> if
        /// possible; otherwise, <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method only returns an instance if a number of conditions are satisfied.
        /// <paramref name="request"/> must represent a request for a seed string, and
        /// <paramref name="context"/> must be able to create a string.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var seededRequest = request as SeededRequest;
            if (seededRequest == null ||
                (!seededRequest.Request.Equals(typeof(string))))
            {
                return new NoSpecimen();
            }

            var seed = seededRequest.Seed as string;
            if (seed == null)
            {
                return new NoSpecimen();
            }

            var containerResult = context.Resolve(typeof(string));
            if (containerResult is NoSpecimen)
            {
                return containerResult;
            }

            return seed + containerResult;
        }
    }
}
