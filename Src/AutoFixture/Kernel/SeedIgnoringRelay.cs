using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Unwraps a request for a <see cref="SeededRequest"/> to a request for its
    /// <see cref="SeededRequest.Request"/> while ignoring the <see cref="SeededRequest.Seed"/>.
    /// </summary>
    public class SeedIgnoringRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates an anonymous value by unwrapping a seeded request and ignoring the seed.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen based on <paramref name="request"/> if possible; otherwise,
        /// <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a seeded request, the Create method unwraps the inner
        /// request and forwards it to <paramref name="context"/>. The seed value is ignored.
        /// </para>
        /// <para>
        /// The purpose of this class is to provide a fallback to handle seeded requests that no
        /// other <see cref="ISpecimenBuilder"/> instances have been able to handle. By ignoring
        /// the seed value, it handles those scenarios where the seed value and the inner request
        /// don't match and can't be combined, ensuring that at least some value is returned.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var seededRequest = request as SeededRequest;
            if (seededRequest == null)
            {
                return new NoSpecimen();
            }

            return context.Resolve(seededRequest.Request);
        }
    }
}
