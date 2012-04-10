using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public static class SpecimenBuilderNode
    {
        public static bool GraphEquals(this ISpecimenBuilderNode first, ISpecimenBuilderNode second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return first.GraphEquals(second, EqualityComparer<ISpecimenBuilder>.Default);
        }

        public static bool GraphEquals(this ISpecimenBuilderNode first, ISpecimenBuilderNode second, IEqualityComparer<ISpecimenBuilder> comparer)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            if (!comparer.Equals(first, second))
                return false;

            using (IEnumerator<ISpecimenBuilder> e1 = first.GetEnumerator(),
                e2 = second.GetEnumerator())
            {
                while (e1.MoveNext())
                {
                    if (!e2.MoveNext())
                        return false;

                    var n1 = e1.Current as ISpecimenBuilderNode;
                    if (n1 != null)
                    {
                        var n2 = e2.Current as ISpecimenBuilderNode;
                        if (n2 != null && (!n1.GraphEquals(n2, comparer)))
                            return false;
                    }
                    else
                    {
                        var n2 = e2.Current as ISpecimenBuilderNode;
                        if (n2 != null && n2.Any())
                            return false;
                        if (!comparer.Equals(e1.Current, e2.Current))
                            return false;
                    }
                }
                if (e2.MoveNext())
                    return false;
            }

            return true;
        }

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
