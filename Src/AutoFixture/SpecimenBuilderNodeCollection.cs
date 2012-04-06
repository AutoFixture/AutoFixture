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
            int i = 0;
            foreach (var s in this.adaptedNode)
            {
                if (item.Equals(s))
                    return i;
                i++;
            }

            return -1;
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(ISpecimenBuilder item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(ISpecimenBuilder item)
        {
            return this.adaptedNode.Contains(item);
        }

        public void CopyTo(ISpecimenBuilder[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(ISpecimenBuilder item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
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
