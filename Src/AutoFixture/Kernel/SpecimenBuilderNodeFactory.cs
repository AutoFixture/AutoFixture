using System;
using AutoFixture.Dsl;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Exposes convenience methods for producing well-known object graphs.
    /// </summary>
    public static class SpecimenBuilderNodeFactory
    {
        /// <summary>
        /// Creates the appropriate initial graph for a
        /// <see cref="NodeComposer{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of specimen to compose.</typeparam>
        /// <returns>
        /// A new <see cref="NodeComposer{T}" /> instance with an appropriate
        /// initial underlying graph.
        /// </returns>
        public static NodeComposer<T> CreateComposer<T>()
        {
            return new NodeComposer<T>(
                SpecimenBuilderNodeFactory.CreateTypedNode(
                    typeof(T),
                    new MethodInvoker(
                        new ModestConstructorQuery())));
        }

        /// <summary>Creates a graph for a typed node.</summary>
        /// <param name="targetType">
        /// The type of the target specimen that this node can produce.
        /// </param>
        /// <param name="factory">
        /// The factory which actually produces specimen instances.
        /// </param>
        /// <returns>
        /// A new <see cref="FilteringSpecimenBuilder" /> instance with an
        /// appropriate underlying graph.
        /// </returns>
        /// <remarks>
        /// While the caller must still supply the <paramref name="factory" />
        /// which produces the specimens, this method wraps that factory in
        /// appropriate filters and relays to ensure that this node handles
        /// and produces only specimens of <paramref name="targetType" />.
        /// </remarks>
        public static FilteringSpecimenBuilder CreateTypedNode(
            Type targetType,
            ISpecimenBuilder factory)
        {
            return new FilteringSpecimenBuilder(
                new CompositeSpecimenBuilder(
                    new NoSpecimenOutputGuard(
                        factory,
                        new InverseRequestSpecification(
                            new SeedRequestSpecification(
                                targetType))),
                    new SeedIgnoringRelay()),
                new OrRequestSpecification(
                    new SeedRequestSpecification(targetType),
                    new ExactTypeSpecification(targetType)));
        }
    }
}
