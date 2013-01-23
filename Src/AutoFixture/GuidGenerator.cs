using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates new <see cref="Guid"/> instances.
    /// </summary>
    public class GuidGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new <see cref="Guid"/> instance.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance.</returns>
        public static Guid CreateAnonymous()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new <see cref="Guid"/> instance.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="Guid"/> instance, if <paramref name="request"/> is a request for a
        /// <see cref="Guid"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(Guid).Equals(request))
            {
                return new NoSpecimen(request);
            }

            return GuidGenerator.CreateAnonymous();
        }
    }
}
