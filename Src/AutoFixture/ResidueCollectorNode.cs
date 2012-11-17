using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A marker class, used to explicitly identify the <i>residue
    /// collector</i> role in an <see cref="ISpecimenBuilderNode" /> graph.
    /// </summary>
    /// <remarks>
    /// The only purpose of this class is to act as an easily identifiable
    /// container. This makes it easier to find the collection of <i>residue
    /// collectors</i> even if it is buried deep in a larger graph.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class ResidueCollectorNode : CompositeSpecimenBuilder
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ResidueCollectorNode" /> class.
        /// </summary>
        /// <param name="builders">
        /// The builders contained within the new instance.
        /// </param>
        public ResidueCollectorNode(params ISpecimenBuilder[] builders)
            : base(builders)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ResidueCollectorNode" /> class.
        /// </summary>
        /// <param name="builders">
        /// The builders contained within the new instance.
        /// </param>
        public ResidueCollectorNode(IEnumerable<ISpecimenBuilder> builders)
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
            return new ResidueCollectorNode(builders);
        }
    }
}
