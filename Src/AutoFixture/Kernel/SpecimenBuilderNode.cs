﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Contains extension methods for working with
    /// <see cref="ISpecimenBuilderNode" /> instances.
    /// </summary>
    public static class SpecimenBuilderNode
    {
        /// <summary>
        /// Determines whether two <see cref="ISpecimenBuilderNode" />
        /// instances are define the same graph.
        /// </summary>
        /// <param name="first">
        /// An <see cref="ISpecimenBuilderNode" /> to compare against
        /// <paramref name="second" />.
        /// </param>
        /// <param name="second">
        /// An <see cref="ISpecimenBuilderNode" /> to compare against
        /// <paramref name="first" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the two
        /// <see cref="ISpecimenBuilderNode" /> define the same graph;
        /// otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Two <see cref="ISpecimenBuilderNode" /> instances define the same
        /// graph if they themselves are equal to each other, and all their
        /// child nodes recursively are equal to each other.
        /// </para>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">first</exception>
        /// <exception cref="System.ArgumentNullException">second</exception>
        /// <seealso cref="GraphEquals(ISpecimenBuilderNode, ISpecimenBuilderNode, IEqualityComparer{ISpecimenBuilder})"/>
        public static bool GraphEquals(this ISpecimenBuilderNode first, ISpecimenBuilderNode second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.GraphEquals(second, EqualityComparer<ISpecimenBuilder>.Default);
        }

        /// <summary>
        /// Determines whether two <see cref="ISpecimenBuilderNode" />
        /// instances are define the same graph.
        /// </summary>
        /// <param name="first">
        /// An <see cref="ISpecimenBuilderNode" /> to compare against
        /// <paramref name="second" />.
        /// </param>
        /// <param name="second">
        /// An <see cref="ISpecimenBuilderNode" /> to compare against
        /// <paramref name="first" />.
        /// </param>
        /// <param name="comparer">
        /// The comparer used to compare each node to another node.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the two
        /// <see cref="ISpecimenBuilderNode" /> define the same graph;
        /// otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Two <see cref="ISpecimenBuilderNode" /> instances define the same
        /// graph if they themselves are equal to each other, and all their
        /// child nodes recursively are equal to each other. Equality is
        /// defined by <paramref name="comparer" />.
        /// </para>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">first</exception>
        /// <exception cref="System.ArgumentNullException">second</exception>
        /// <exception cref="System.ArgumentNullException">comparer</exception>
        /// <seealso cref="GraphEquals(ISpecimenBuilderNode, ISpecimenBuilderNode)"/>
        public static bool GraphEquals(this ISpecimenBuilderNode first, ISpecimenBuilderNode second, IEqualityComparer<ISpecimenBuilder> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

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
