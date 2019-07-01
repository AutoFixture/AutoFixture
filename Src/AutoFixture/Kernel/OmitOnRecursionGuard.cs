using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Recursion handler that returns <see cref="OmitSpecimen" /> at recursion
    /// points.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    [Obsolete("This class will be removed in a future version of AutoFixture. Instead, use an instance of RecursionGuard, composed with an instance of OmitOnRecursionHandler.", true)]
    public class OmitOnRecursionGuard : RecursionGuard
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="OmitOnRecursionGuard" /> class.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        public OmitOnRecursionGuard(ISpecimenBuilder builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="OmitOnRecursionGuard" /> class.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        /// <param name="comparer">
        /// An <see cref="IEqualityComparer" /> implementation to use when
        /// comparing requests to determine recursion.
        /// </param>
        public OmitOnRecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
            : base(builder, comparer)
        {
        }

        /// <summary>
        /// Handles a request that would cause recursion.
        /// </summary>
        /// <param name="request">The recursion-causing request.</param>
        /// <returns>
        /// An <see cref="OmitSpecimen" /> instance.
        /// </returns>
        [Obsolete("This class will be removed in a future version of AutoFixture. Instead, use an instance of RecursionGuard, composed with an instance of OmitOnRecursionHandler.", true)]
        public override object HandleRecursiveRequest(object request)
        {
            return new OmitSpecimen();
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            if (builders == null) throw new ArgumentNullException(nameof(builders));

            var composedBuilder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new OmitOnRecursionGuard(composedBuilder, this.Comparer);
        }
    }
}
