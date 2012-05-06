using System;
using System.Globalization;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Prevents a decorated <see cref="ISpecimenBuilder"/> from returning a
    /// <see cref="NoSpecimen"/> instance.
    /// </summary>
    public class NoSpecimenOutputGuard : ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilder builder;
        private readonly IRequestSpecification specification;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpecimenOutputGuard"/> class with an 
        /// <see cref="ISpecimenBuilder"/> to decorate.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        public NoSpecimenOutputGuard(ISpecimenBuilder builder)
            : this(builder, new TrueRequestSpecification())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpecimenOutputGuard"/> class with an
        /// <see cref="ISpecimenBuilder"/> to decorate and an <see cref="IRequestSpecification"/>
        /// that is used to determine whether an exception should be thrown based on the request.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        /// <param name="specification">The specification.</param>
        public NoSpecimenOutputGuard(ISpecimenBuilder builder, IRequestSpecification specification)
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
        /// <value>The <see cref="ISpecimenBuilder"/> supplied via the constructor.</value>
        /// <seealso cref="NoSpecimenOutputGuard(ISpecimenBuilder)"/>
        /// <seealso cref="NoSpecimenOutputGuard(ISpecimenBuilder, IRequestSpecification)"/>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Gets the specification that is used to determine whether an exception should be thrown
        /// for a request that returns a <see cref="NoSpecimen"/> instance.
        /// </summary>
        /// <value>The <see cref="IRequestSpecification"/> supplied via the constructor.</value>
        /// <seealso cref="NoSpecimenOutputGuard(ISpecimenBuilder)"/>
        /// <seealso cref="NoSpecimenOutputGuard(ISpecimenBuilder, IRequestSpecification)"/>
        public IRequestSpecification Specification
        {
            get { return this.specification; }
        }

        /// <summary>
        /// Creates a new specimen by delegating to the decorated <see cref="Builder"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise an exception is thrown or a
        /// <see cref="NoSpecimen"/> instance is returned.
        /// </returns>
        /// <exception cref="ObjectCreationException">
        /// The decorated <see cref="Builder"/> returned a <see cref="NoSpecimen"/> result and
        /// <see cref="Specification"/> returned <see langword="true"/> for
        /// <paramref name="request"/>.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ISpecimenBuilder", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IFactoryComposer", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        public object Create(object request, ISpecimenContext context)
        {
            var result = this.Builder.Create(request, context);
            if (result is NoSpecimen 
                && this.specification.IsSatisfiedBy(request))
            {
                throw new ObjectCreationException(string.Format(CultureInfo.CurrentCulture, "The decorated ISpecimenBuilder could not create a specimen based on the request: {0}. This can happen if the request represents an interface or abstract class; if this is the case, register an ISpecimenBuilder that can create specimens based on the request. If this happens in a strongly typed Build<T> expression, try supplying a factory using one of the IFactoryComposer<T> methods.", request));
            }
            return result;
        }

        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new NoSpecimenOutputGuard(composedBuilder, this.specification);
        }

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
