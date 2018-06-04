using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Contains extension methods for working with
    /// <see cref="ISpecimenBuilderNode" /> instances.
    /// </summary>
    public static class SpecimenBuilderNode
    {
        /// <summary>
        /// Determines whether two <see cref="ISpecimenBuilderNode" /> instances are define the same graph.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Two <see cref="ISpecimenBuilderNode" /> instances define the same
        /// graph if they themselves are equal to each other, and all their
        /// child nodes recursively are equal to each other.
        /// </para>
        /// </remarks>
        public static bool GraphEquals(this ISpecimenBuilderNode first, ISpecimenBuilderNode second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return first.GraphEquals(second, EqualityComparer<ISpecimenBuilder>.Default);
        }

        /// <summary>
        /// Determines whether two <see cref="ISpecimenBuilderNode" /> instances are define the same graph.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Two <see cref="ISpecimenBuilderNode" /> instances define the same
        /// graph if they themselves are equal to each other, and all their
        /// child nodes recursively are equal to each other. Equality is
        /// defined by <paramref name="comparer" />.
        /// </para>
        /// </remarks>
        public static bool GraphEquals(this ISpecimenBuilderNode first, ISpecimenBuilderNode second, IEqualityComparer<ISpecimenBuilder> comparer)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            if (!comparer.Equals(first, second))
                return false;

            using (IEnumerator<ISpecimenBuilder> e1 = first.GetEnumerator(), e2 = second.GetEnumerator())
            {
                while (e1.MoveNext())
                {
                    if (!e2.MoveNext())
                        return false;

                    var n1 = e1.Current as ISpecimenBuilderNode;
                    var n2 = e2.Current as ISpecimenBuilderNode;
                    if (n1 != null && n2 != null)
                    {
                        if (!n1.GraphEquals(n2, comparer))
                            return false;
                    }
                    else
                    {
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

        internal static ISpecimenBuilderNode ReplaceNodes(
            this ISpecimenBuilderNode graph,
            Func<ISpecimenBuilderNode, ISpecimenBuilderNode> with,
            Func<ISpecimenBuilderNode, bool> when)
        {
            if (when(graph))
                return with(graph);

            var nodes = from b in graph
                        let n = b as ISpecimenBuilderNode
                        select n != null ? n.ReplaceNodes(with, when) : b;
            return graph.Compose(nodes);
        }

        /// <summary>
        /// Finds the first node in the passed graph that matches the specified predicate.
        /// If nothing is found - null is returned.
        /// </summary>
        internal static ISpecimenBuilderNode FindFirstNodeOrDefault(this ISpecimenBuilderNode graph, Func<ISpecimenBuilderNode, bool> predicate)
        {
            if (predicate.Invoke(graph))
                return graph;

            foreach (var builder in graph)
            {
                if (builder is ISpecimenBuilderNode builderNode)
                {
                    var result = FindFirstNodeOrDefault(builderNode, predicate);
                    if (result != null) return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first node in the graph that matches the specified predicate.
        /// If no node is present - fails with exception.
        /// </summary>
        internal static ISpecimenBuilderNode FindFirstNode(this ISpecimenBuilderNode graph, Func<ISpecimenBuilderNode, bool> predicate)
        {
            var result = graph.FindFirstNodeOrDefault(predicate);
            if (result == null)
            {
                throw new InvalidOperationException("Unable to find node matching the specified predicate.");
            }

            return result;
        }
    }
}
