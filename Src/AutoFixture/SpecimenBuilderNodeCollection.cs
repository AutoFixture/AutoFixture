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

        public event EventHandler<SpecimenBuilderNodeEventArgs> GraphChanged;

        public int IndexOf(ISpecimenBuilder item)
        {
            return this.adaptedNode.IndexOf(item);
        }

        public void Insert(int index, ISpecimenBuilder item)
        {
            this.Mutate(this.adaptedNode.Insert(index, item));
        }

        public void RemoveAt(int index)
        {
            this.Mutate(this.adaptedNode.RemoveAt(index));
        }

        public ISpecimenBuilder this[int index]
        {
            get
            {
                return this.adaptedNode.ElementAt(index);
            }
            set
            {
                this.Mutate(this.adaptedNode.SetItem(index, value));
            }
        }

        public void Add(ISpecimenBuilder item)
        {
            this.Mutate(this.adaptedNode.Concat(new[] { item }));
        }

        public void Clear()
        {
            this.Mutate(Enumerable.Empty<ISpecimenBuilder>());
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

        public ISpecimenBuilderNode Graph
        {
            get { return this.graph; }
        }

        protected virtual void OnGraphChanged(SpecimenBuilderNodeEventArgs e)
        {
            var handler = this.GraphChanged;
            if (handler != null)
                handler(this, e);
        }

        private void Mutate(IEnumerable<ISpecimenBuilder> builders)
        {
            this.graph = this.graph.ReplaceNode(with: builders, when: this.isAdaptedBuilder);
            this.adaptedNode = this.SelectAdaptedNodes(this.graph).Single();

            this.OnGraphChanged(new SpecimenBuilderNodeEventArgs(this.graph));
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
    }
}
