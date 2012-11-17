using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Decorates an <see cref="ISpecimenBuilder"/> and filters requests so that only some requests
    /// are passed through to the decorated builder.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class FilteringSpecimenBuilder : ISpecimenBuilderNode
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

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>
        /// A specimen created by the decorated <see cref="ISpecimenBuilder"/> if the filter allows
        /// the request through; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!this.specification.IsSatisfiedBy(request))
            {
                return new NoSpecimen(request);
            }

            return this.builder.Create(request, context);
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new FilteringSpecimenBuilder(composedBuilder, this.specification);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{ISpecimenBuilder}" /> that can be used to
        /// iterate through the collection.
        /// </returns>
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.builder;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
