using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class SpecimenBuilderNodeCollection : IList<ISpecimenBuilder>
    {
        private ISpecimenBuilderNode graph;
        private readonly Func<ISpecimenBuilderNode, bool> isAdaptedBuilder;
        private ISpecimenBuilderNode adaptedNode;

        public SpecimenBuilderNodeCollection(
            ISpecimenBuilderNode graph,
            Func<ISpecimenBuilderNode, bool> adaptedBuilderPredicate)
        {
            this.graph = graph;
            this.isAdaptedBuilder = adaptedBuilderPredicate;
            this.adaptedNode = this.SelectAdaptedNodes(this.graph).Single();
        }

        public int IndexOf(ISpecimenBuilder item)
        {
            return this.adaptedNode.IndexOf(item);
        }

        public void Insert(int index, ISpecimenBuilder item)
        {
            var builders = this.adaptedNode.Insert(index, item);
            this.graph = this.ReplaceAdaptedWith(this.graph, builders);
            this.adaptedNode = this.SelectAdaptedNodes(this.graph).Single();
        }

        public void RemoveAt(int index)
        {
            var builders = this.adaptedNode.RemoveAt(index);
            this.graph = this.ReplaceAdaptedWith(this.graph, builders);
            this.adaptedNode = this.SelectAdaptedNodes(this.graph).Single();
        }

        public ISpecimenBuilder this[int index]
        {
            get
            {
                return this.adaptedNode.ElementAt(index);
            }
            set
            {
                var builders = this.adaptedNode.SetItem(index, value);
                this.graph = this.ReplaceAdaptedWith(this.graph, builders);
                this.adaptedNode = this.SelectAdaptedNodes(this.graph).Single();
            }
        }

        public void Add(ISpecimenBuilder item)
        {
            var builders = this.adaptedNode.Concat(new[] { item });
            this.graph = this.ReplaceAdaptedWith(this.graph, builders);
            this.adaptedNode = this.SelectAdaptedNodes(this.graph).Single();
        }

        public void Clear()
        {
            this.graph = this.ReplaceAdaptedWith(this.graph, Enumerable.Empty<ISpecimenBuilder>());
            this.adaptedNode = this.SelectAdaptedNodes(this.graph).Single();
        }

        public bool Contains(ISpecimenBuilder item)
        {
            return this.adaptedNode.Contains(item);
        }

        public void CopyTo(ISpecimenBuilder[] array, int arrayIndex)
        {
            this.adaptedNode.ToArray().CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.adaptedNode.Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ISpecimenBuilder item)
        {
            var contained = this.Contains(item);

            var index = this.IndexOf(item);
            this.RemoveAt(index);

            return contained;
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            return this.adaptedNode.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private IEnumerable<ISpecimenBuilderNode> SelectAdaptedNodes(ISpecimenBuilderNode graph)
        {
            if (this.isAdaptedBuilder(graph))
                yield return graph;

            foreach (var g in graph)
            {
                var n = g as ISpecimenBuilderNode;
                if (n != null)
                    foreach (var n1 in this.SelectAdaptedNodes(n))
                        yield return n1;
            }
        }

        private ISpecimenBuilderNode ReplaceAdaptedWith(ISpecimenBuilderNode graph, IEnumerable<ISpecimenBuilder> builders)
        {
            if (this.isAdaptedBuilder(graph))
                return graph.Compose(builders);

            var nodes = from g in graph
                        let n = g as ISpecimenBuilderNode
                        select n != null ? this.ReplaceAdaptedWith(n, builders) : g;
            return graph.Compose(nodes);
        }
    }
}
