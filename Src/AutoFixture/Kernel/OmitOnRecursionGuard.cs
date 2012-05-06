using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Recursion handler that returns <see cref="OmitSpecimen" /> at recursion
    /// points.
    /// </summary>
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
        public override object HandleRecursiveRequest(object request)
        {
            return new OmitSpecimen();
        }

        public override ISpecimenBuilderNode Compose(
            IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = 
                CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new OmitOnRecursionGuard(composedBuilder, this.Comparer);
        }
    }
}
