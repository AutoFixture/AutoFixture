using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Collections.ObjectModel;

namespace Ploeh.AutoFixture
{
    public class SingletonSpecimenBuilderNodeStackCollectionAdapter : Collection<ISpecimenBuilderTransformation>
    {
        private ISpecimenBuilderNode graph;
        private readonly Func<ISpecimenBuilderNode, bool> isWrappedGraph;

        public SingletonSpecimenBuilderNodeStackCollectionAdapter(ISpecimenBuilderNode graph, Func<ISpecimenBuilderNode, bool> wrappedGraphPredicate)
        {
            this.graph = graph;
            this.isWrappedGraph = wrappedGraphPredicate;
        }

        public event EventHandler<SpecimenBuilderNodeEventArgs> GraphChanged;

        public ISpecimenBuilderNode Graph
        {
            get { return this.graph; }
        }

        protected override void ClearItems()
        {
            var wasNotEmpty = this.Count > 0;
            base.ClearItems();
            if (wasNotEmpty)
                this.UpdateGraph();
        }

        protected override void InsertItem(int index, ISpecimenBuilderTransformation item)
        {
            base.InsertItem(index, item);
            this.UpdateGraph();            
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            this.UpdateGraph();
        }

        protected override void SetItem(int index, ISpecimenBuilderTransformation item)
        {
            base.SetItem(index, item);
            this.UpdateGraph();
        }

        protected virtual void OnGraphChanged(SpecimenBuilderNodeEventArgs e)
        {
            var handler = this.GraphChanged;
            if (handler != null)
                handler(this, e);
        }

        private void UpdateGraph()
        {
            ISpecimenBuilder g = this.graph.SelectNodes(this.isWrappedGraph).First();
            var builder = this.Aggregate(g, (b, t) => t.Transform(b));
            this.graph = (ISpecimenBuilderNode)builder;

            this.OnGraphChanged(new SpecimenBuilderNodeEventArgs(this.graph));
        }
    }
}
