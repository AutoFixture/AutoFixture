using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A marker class, used to explicitly identify the root of the
    /// <i>behaviors</i> role in an <see cref="ISpecimenBuilderNode" /> graph.
    /// </summary>
    /// <remarks>
    /// The only purpose of this class is to act as an easily identifiable
    /// container. This makes it easier to find the root of the of
    /// <i>behaviors</i> even if it is buried deep in a larger graph.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class BehaviorRoot : ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilder builder;

        public BehaviorRoot(ISpecimenBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = 
                CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new BehaviorRoot(composedBuilder);
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.builder.Create(request, context);
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.builder;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }
    }
}
