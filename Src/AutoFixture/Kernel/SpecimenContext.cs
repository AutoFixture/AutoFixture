using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Default implementation of <see cref="ISpecimenContext"/>.
    /// </summary>
    public class SpecimenContext : ISpecimenContext
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SpecimenContext"/> with the supplied
        /// <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="builder">The builder that will handle requests.</param>
        public SpecimenContext(ISpecimenBuilder builder)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the <see cref="ISpecimenBuilder"/> contained by the instance.
        /// </summary>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Creates an anonymous variable (specimen) based on a request by delegating the request
        /// to its contained <see cref="Builder"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <returns>The result of a request to the contained <see cref="Builder"/>.</returns>
        public object Resolve(object request)
        {
            return this.Builder.Create(request, this);
        }
    }
}
