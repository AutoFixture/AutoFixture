using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A marker class, used to explicitly identify the target for
    /// <i>Auto-properties</i> in an <see cref="ISpecimenBuilderNode" /> graph.
    /// </summary>
    /// <remarks>
    /// The only purpose of this class is to act as an easily identifiable
    /// container. This makes it easier to find the target for
    /// <i>Auto-properties</i> even if it is buried deep in a larger graph.
    /// </remarks>
    public class AutoPropertiesTarget : CompositeSpecimenBuilder
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AutoPropertiesTarget" /> class.
        /// </summary>
        /// <param name="builders">
        /// The builders contained within the new instance.
        /// </param>
        public AutoPropertiesTarget(params ISpecimenBuilder[] builders)
            : base(builders)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AutoPropertiesTarget" /> class.
        /// </summary>
        /// <param name="builders">
        /// The builders contained within the new instance.
        /// </param>
        public AutoPropertiesTarget(IEnumerable<ISpecimenBuilder> builders)
            : base(builders)
        {
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new AutoPropertiesTarget(builders);
        }
    }
}
