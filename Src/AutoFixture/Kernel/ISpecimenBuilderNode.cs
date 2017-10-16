using System.Collections.Generic;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A node in a graph of <see cref="ISpecimenBuilder" /> instances.
    /// </summary>
    /// <remarks>
    /// <para>
    /// AutoFixture's kernel basically works as a 'Graph of Responsibility'.
    /// Each <see cref="ISpecimenBuilder" /> gets a chance to handle a request,
    /// and if it can't do that, the next ISpecimenBuilder is asked to handle
    /// the request. When an ISpecimenBuilder instance returns a useful
    /// specimen, that instance is returned and the results (if any) from
    /// lower-priority ISpecimenBuilder instances are ignored.
    /// </para>
    /// <para>
    /// In theory, one could order all ISpecimenBuilder instances in a flat
    /// Chain of Responsibility, but instead, the ISpecimenBuilderNode
    /// interface defines the responsibilities of a 'parent' node in a deeper
    /// graph. Each ISpecimenBuilder node constitute an intermediate node in a
    /// graph, while itself being a parent to other nodes. In the degenerate
    /// case when an ISpecimenBuilderNode has no child nodes, it effectively
    /// becomes a leaf node. Otherwise, leaf nodes are typically
    /// ISpecimenBuilder instances.
    /// </para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public interface ISpecimenBuilderNode : ISpecimenBuilder, IEnumerable<ISpecimenBuilder>
    {
        /// <summary>
        /// Composes the supplied builders.
        /// </summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>A <see cref="ISpecimenBuilderNode" /> instance.</returns>
        /// <remarks>
        /// <para>
        /// Note to implementers:
        /// </para>
        /// <para>
        /// The intent of this method is to compose the supplied
        /// <paramref name="builders" /> into a new instance of the type
        /// implementing <see cref="ISpecimenBuilderNode" />. Thus, the
        /// concrete return type is expected to the same type as the type
        /// implementing the method. However, it is not considered a failure to
        /// deviate from this idiom - it would just not be a mainstream
        /// implementation.
        /// </para>
        /// <para>
        /// The returned instance is normally expected to contain the builders
        /// supplied as an argument, but again this is not strictly required.
        /// The implementation may decide to filter the sequence or add to it
        /// during composition.
        /// </para>
        /// </remarks>
        ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders);
    }
}
