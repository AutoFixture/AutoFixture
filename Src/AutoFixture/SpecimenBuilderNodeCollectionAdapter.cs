using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class SpecimenBuilderNodeCollectionAdapter : IList<ISpecimenBuilder>
    {
        private ISpecimenBuilderNode graph;
        private readonly Func<ISpecimenBuilderNode, bool> isAdaptedBuilder;
        private IEnumerable<ISpecimenBuilder> adaptedBuilders;

        public SpecimenBuilderNodeCollectionAdapter(
            ISpecimenBuilderNode graph,
            Func<ISpecimenBuilderNode, bool> adaptedBuilderPredicate)
        {
            this.graph = graph;
            this.isAdaptedBuilder = adaptedBuilderPredicate;
            this.adaptedBuilders = this.graph.SelectNodes(this.isAdaptedBuilder).Single();
        }

        public event EventHandler<SpecimenBuilderNodeEventArgs> GraphChanged;

        public int IndexOf(ISpecimenBuilder item)
        {
            return this.adaptedBuilders.IndexOf(item);
        }

        public void Insert(int index, ISpecimenBuilder item)
        {
            this.Mutate(this.adaptedBuilders.Insert(index, item));
        }

        public void RemoveAt(int index)
        {
            this.Mutate(this.adaptedBuilders.RemoveAt(index));
        }

        public ISpecimenBuilder this[int index]
        {
            get
            {
                return this.adaptedBuilders.ElementAt(index);
            }
            set
            {
                this.Mutate(this.adaptedBuilders.SetItem(index, value));
            }
        }

        public void Add(ISpecimenBuilder item)
        {
            this.Mutate(this.adaptedBuilders.Concat(new[] { item }));
        }

        public void Clear()
        {
            this.Mutate(Enumerable.Empty<ISpecimenBuilder>());
        }

        public bool Contains(ISpecimenBuilder item)
        {
            return this.adaptedBuilders.Contains(item);
        }

        public void CopyTo(ISpecimenBuilder[] array, int arrayIndex)
        {
            this.adaptedBuilders.ToArray().CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.adaptedBuilders.Count(); }
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
            return this.adaptedBuilders.GetEnumerator();
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
            this.graph = this.graph.ReplaceNodes(with: builders, when: this.isAdaptedBuilder);
            this.adaptedBuilders = this.graph.SelectNodes(this.isAdaptedBuilder).Single();

            this.OnGraphChanged(new SpecimenBuilderNodeEventArgs(this.graph));
        }
    }
}
