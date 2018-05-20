using System;
using System.Text;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Handles creation requests for <see cref="Encoding"/> instances,
    /// returning always the same <see cref="Encoding.UTF8"/>.
    /// </summary>
    [Obsolete("Please use a 'AutoFixture.Kernel.FilteringSpecimenBuilder' instead.", true)]
    public class Utf8EncodingGenerator : ISpecimenBuilder
    {
        private readonly ExactTypeSpecification encodingTypeSpecification = new ExactTypeSpecification(typeof(Encoding));

        /// <summary>
        /// Generates a Encoding specimen based on a <see cref="Encoding"/> type request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// The generated Encoding will always be the same as <see cref="Encoding.UTF8"/>.
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                return new NoSpecimen();
            }

            if (!this.encodingTypeSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            return Encoding.UTF8;
        }
    }
}