using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    internal static class SpecimenBuilderNode
    {
        internal static ISpecimenBuilderNode ReplaceNodes(
            this ISpecimenBuilderNode graph,
            IEnumerable<ISpecimenBuilder> with,
            Func<ISpecimenBuilderNode, bool> when)
        {
            if (when(graph))
                return graph.Compose(with);

            var nodes = from b in graph
                        let n = b as ISpecimenBuilderNode
                        select n != null ? n.ReplaceNodes(with, when) : b;
            return graph.Compose(nodes);
        }

        internal static ISpecimenBuilderNode ReplaceNodes(
            this ISpecimenBuilderNode graph,
            ISpecimenBuilderNode with,
            Func<ISpecimenBuilderNode, bool> when)
        {
            if (when(graph))
                return with;

            var nodes = from b in graph
                        let n = b as ISpecimenBuilderNode
                        select n != null ? n.ReplaceNodes(with, when) : b;
            return graph.Compose(nodes);
        }

        internal static IEnumerable<ISpecimenBuilderNode> Parents(
            this ISpecimenBuilderNode graph,
            Func<ISpecimenBuilder, bool> predicate)
        {
            foreach (var b in graph)
            {
                if (predicate(b))
                    yield return graph;

                var n = b as ISpecimenBuilderNode;
                if (n != null)
                {
                    foreach (var n1 in n.Parents(predicate))
                        yield return n1;
                }
            }
        }

        internal static IEnumerable<ISpecimenBuilderNode> SelectNodes(
            this ISpecimenBuilderNode graph,
            Func<ISpecimenBuilderNode, bool> predicate)
        {
            if (predicate(graph))
                yield return graph;

            foreach (var b in graph)
            {
                var n = b as ISpecimenBuilderNode;
                if (n != null)
                    foreach (var n1 in n.SelectNodes(predicate))
                        yield return n1;
            }
        }
    }
}
