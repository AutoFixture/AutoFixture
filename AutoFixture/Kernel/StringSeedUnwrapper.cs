using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Unwraps a request for a string <see cref="SeededRequest"/> to a request for a string and
    /// prefixes the seed to the result.
    /// </summary>
    public class StringSeedUnwrapper : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates an anonymous string based on a seed.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A string with the seed prefixed to a string created by <paramref name="container"/> if
        /// possible; otherwise, <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method only returns an instance if a number of conditions are satisfied.
        /// <paramref name="request"/> must represent a request for a seed string, and
        /// <paramref name="container"/> must be able to create a string.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var seededRequest = request as SeededRequest;
            if (seededRequest == null ||
                seededRequest.Request != typeof(string))
            {
                return new NoSpecimen(request);
            }

            var seed = seededRequest.Seed as string;
            if (seed == null)
            {
                return new NoSpecimen(request);
            }

            var containerResult = container.Create(typeof(string));
            if (containerResult is NoSpecimen)
            {
                return containerResult;
            }

            return seed + containerResult;
        }

        #endregion
    }
}
