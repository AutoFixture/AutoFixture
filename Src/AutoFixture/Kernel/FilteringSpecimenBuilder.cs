using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Decorates an <see cref="ISpecimenBuilder"/> and filters requests so that only some requests
    /// are passed through to the decorated builder.
    /// </summary>
    public class FilteringSpecimenBuilder : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;
        private readonly IRequestSpecification specification;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilteringSpecimenBuilder"/> class.
        /// </summary>
        /// <param name="builder">A specimen builder to decorate.</param>
        /// <param name="specification">
        /// A specification that determines whether <paramref name="builder"/> will receive the request.
        /// </param>
        public FilteringSpecimenBuilder(ISpecimenBuilder builder, IRequestSpecification specification)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }        
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }

            this.builder = builder;
            this.specification = specification;
        }

        /// <summary>
        /// Gets the decorated builder.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Gets the specification that determines whether <see cref="Builder"/> will be invoked or
        /// not.
        /// </summary>
        public IRequestSpecification Specification
        {
            get { return this.specification; }
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created by the decorated <see cref="ISpecimenBuilder"/> if the filter allows
        /// the request through; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext container)
        {
            if (!this.specification.IsSatisfiedBy(request))
            {
                return new NoSpecimen(request);
            }

            return this.builder.Create(request, container);
        }

        #endregion
    }
}
