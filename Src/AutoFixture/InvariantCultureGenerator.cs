using System;
using System.Globalization;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Handles creation requests for <see cref="CultureInfo"/> instances,
    /// returning always the same <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Obsolete("Please use a 'AutoFixture.Kernel.FilteringSpecimenBuilder' instead.", true)]
    public class InvariantCultureGenerator : ISpecimenBuilder
    {
        private readonly ExactTypeSpecification cultureTypeSpecification
            = new ExactTypeSpecification(typeof(CultureInfo));

        /// <summary>
        /// Generates a CultureInfo specimen based on a <see cref="CultureInfo"/> type request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>>
        /// The generated CultureInfo will always be the same as <see cref="CultureInfo.InvariantCulture"/>.
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
                return new NoSpecimen();

            if (!this.cultureTypeSpecification.IsSatisfiedBy(request))
                return new NoSpecimen();

            return CultureInfo.InvariantCulture;
        }
    }
}