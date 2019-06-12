using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// A marker class, used to explicitly identify builders related to the data annotation handling.
    /// </summary>
    /// <remarks>
    /// The only purpose of this class is to act as an easily identifiable container.
    /// This makes it easier to find the collection of data annotation related nodes in the customization list.
    /// </remarks>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class DataAnnotationsSupportNode : ISpecimenBuilderNode
    {
        /// <summary>
        /// Builder used to handle the incoming requests.
        /// </summary>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DataAnnotationsSupportNode"/>.
        /// </summary>
        /// <param name="builder">Builder that handles all the data annotation related requests.</param>
        public DataAnnotationsSupportNode(ISpecimenBuilder builder)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            return this.Builder.Create(request, context);
        }

        /// <inheritdoc />
        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            if (builders == null) throw new ArgumentNullException(nameof(builders));

            return new DataAnnotationsSupportNode(CompositeSpecimenBuilder.ComposeIfMultiple(builders));
        }

        /// <inheritdoc />
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.Builder;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}