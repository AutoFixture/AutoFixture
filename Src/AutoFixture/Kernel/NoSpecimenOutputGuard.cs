using System;
using System.Collections.Generic;
using System.Globalization;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Prevents a decorated <see cref="ISpecimenBuilder"/> from returning a
    /// <see cref="NoSpecimen"/> instance.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class NoSpecimenOutputGuard : ISpecimenBuilderNode
    {
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
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.Specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }

        /// <summary>
        /// Gets the decorated builder.
        /// </summary>
        /// <value>The <see cref="ISpecimenBuilder"/> supplied via the constructor.</value>
        /// <seealso cref="NoSpecimenOutputGuard(ISpecimenBuilder)"/>
        /// <seealso cref="NoSpecimenOutputGuard(ISpecimenBuilder, IRequestSpecification)"/>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Gets the specification that is used to determine whether an exception should be thrown
        /// for a request that returns a <see cref="NoSpecimen"/> instance.
        /// </summary>
        /// <value>The <see cref="IRequestSpecification"/> supplied via the constructor.</value>
        /// <seealso cref="NoSpecimenOutputGuard(ISpecimenBuilder)"/>
        /// <seealso cref="NoSpecimenOutputGuard(ISpecimenBuilder, IRequestSpecification)"/>
        public IRequestSpecification Specification { get; }

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
        public object Create(object request, ISpecimenContext context)
        {
            var result = this.Builder.Create(request, context);
            if (result is NoSpecimen
                && this.Specification.IsSatisfiedBy(request))
            {
                throw new ObjectCreationException(string.Format(CultureInfo.CurrentCulture, "The decorated ISpecimenBuilder could not create a specimen based on the request: {0}. This can happen if the request represents an interface or abstract class; if this is the case, register an ISpecimenBuilder that can create specimens based on the request. If this happens in a strongly typed Build<T> expression, try supplying a factory using one of the IFactoryComposer<T> methods.", request));
            }
            return result;
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            if (builders == null) throw new ArgumentNullException(nameof(builders));

            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new NoSpecimenOutputGuard(composedBuilder, this.Specification);
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
            yield return this.Builder;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
