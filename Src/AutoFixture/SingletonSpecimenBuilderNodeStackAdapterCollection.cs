using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// A collection of <see cref="ISpecimenBuilderTransformation" />
    /// instances, which can be used to produce a stack of singleton
    /// <see cref="ISpecimenBuilderNode" /> instances at the root of any
    /// ISpecimenBuilderNode graph.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Apart from containing a collection of
    /// <see cref="ISpecimenBuilderTransformation" /> instances, the
    /// SingletonSpecimenBuilderNodeStackCollectionAdapter also holds a
    /// reference to a <see cref="ISpecimenBuilderNode" /> graph. Every time an
    /// ISpecimenBuilderTransformation instance is added to the collection, it
    /// is also applied to the graph by invoking
    /// <see cref="ISpecimenBuilderTransformation.Transform(ISpecimenBuilder)" />
    /// against the existing graph. This doesn't change the existing graph, but
    /// produces a new graph, which then replaces the previous graph. The
    /// current graph is always available via the <see cref="Graph" />
    /// property.
    /// </para>
    /// <para>
    /// For all mutating operations on the collection, the
    /// SingletonSpecimenBuilderNodeStackCollectionAdapter replaces its
    /// internal graph with a new graph that encapsulates the new state. The
    /// original graph is not mutated, but when a seemingly mutating method is
    /// invoked, the <see cref="GraphChanged" /> event is raised.
    /// </para>
    /// </remarks>
    public class SingletonSpecimenBuilderNodeStackAdapterCollection : Collection<ISpecimenBuilderTransformation>
    {
        private readonly Func<ISpecimenBuilderNode, bool> isWrappedGraph;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SingletonSpecimenBuilderNodeStackAdapterCollection" />
        /// class.
        /// </summary>
        /// <param name="graph">
        /// The base graph upon which the contained
        /// <see cref="ISpecimenBuilderTransformation"/> are applied.
        /// </param>
        /// <param name="wrappedGraphPredicate">
        /// A predicate which identifies the wrapped graph.
        /// </param>
        /// <param name="transformations">
        /// The transformations to apply to <paramref name="graph" />.
        /// </param>
        /// <remarks>
        /// <para>
        /// The <paramref name="transformations" /> and subsequent
        /// transformations added to the instance are applied to a base graph.
        /// The base graph is found by looking for a node within the graph
        /// where <paramref name="wrappedGraphPredicate" /> returns true.
        /// </para>
        /// </remarks>
        public SingletonSpecimenBuilderNodeStackAdapterCollection(
            ISpecimenBuilderNode graph,
            Func<ISpecimenBuilderNode, bool> wrappedGraphPredicate,
            params ISpecimenBuilderTransformation[] transformations)
        {
            if (transformations == null) throw new ArgumentNullException(nameof(transformations));

            this.Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            this.isWrappedGraph = wrappedGraphPredicate ?? throw new ArgumentNullException(nameof(wrappedGraphPredicate));

            foreach (var t in transformations)
                this.Add(t);
        }

        /// <summary>
        /// Occurs when this instance changes its internal view of the
        /// encapsulated <see cref="ISpecimenBuilderNode" /> graph.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When this instance performs a mutating action, it replaces its
        /// previously encapsulated <see cref="ISpecimenBuilderNode" /> graph
        /// with a new graph where the change has been applied. The event
        /// arguments raised with the event contains a reference to the new
        /// graph.
        /// </para>
        /// </remarks>
        public event EventHandler<SpecimenBuilderNodeEventArgs> GraphChanged;

        /// <summary>Gets the full graph.</summary>
        /// <value>
        /// The full graph produced by applying the collection of
        /// <see cref="ISpecimenBuilderTransformation" /> instances to the base
        /// graph.
        /// </value>
        public ISpecimenBuilderNode Graph { get; private set; }

        /// <summary>Removes all items from the collection.</summary>
        /// <remarks>
        /// <para>
        /// Since this is a potentially mutating operation it may raise the
        /// <see cref="GraphChanged" /> event.
        /// </para>
        /// </remarks>
        protected override void ClearItems()
        {
            var wasNotEmpty = this.Count > 0;
            base.ClearItems();
            if (wasNotEmpty)
                this.UpdateGraph();
        }

        /// <summary>
        /// Inserts an element into the collection at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="index" /> should be
        /// inserted.
        /// </param>
        /// <param name="item">The item to insert.</param>
        /// <remarks>
        /// <para>
        /// Since this is a mutating operation it raises the
        /// <see cref="GraphChanged" /> event.
        /// </para>
        /// </remarks>
        protected override void InsertItem(int index, ISpecimenBuilderTransformation item)
        {
            base.InsertItem(index, item);
            this.UpdateGraph();
        }

        /// <summary>
        /// Removes the element at the specified index of the collection.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to remove.
        /// </param>
        /// <remarks>
        /// <para>
        /// Since this is a mutating operation it raises the
        /// <see cref="GraphChanged" /> event.
        /// </para>
        /// </remarks>
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            this.UpdateGraph();
        }

        /// <summary>Replaces the element at the specified index.</summary>
        /// <param name="index">
        /// The zero-based index of the element to replace.
        /// </param>
        /// <param name="item">
        /// The new value for the element at the specified index.
        /// </param>
        /// <remarks>
        /// <para>
        /// Since this is a mutating operation it raises the
        /// <see cref="GraphChanged" /> event.
        /// </para>
        /// </remarks>
        protected override void SetItem(int index, ISpecimenBuilderTransformation item)
        {
            base.SetItem(index, item);
            this.UpdateGraph();
        }

        /// <summary>Raises the <see cref="GraphChanged" /> event.</summary>
        /// <param name="e">
        /// The <see cref="SpecimenBuilderNodeEventArgs" /> instance containing
        /// the event data.
        /// </param>
        protected virtual void OnGraphChanged(SpecimenBuilderNodeEventArgs e)
        {
            var handler = this.GraphChanged;
            if (handler != null)
                handler(this, e);
        }

        private void UpdateGraph()
        {
            ISpecimenBuilderNode g = this.Graph.FindFirstNode(this.isWrappedGraph);
            ISpecimenBuilderNode builder = this.Aggregate(g, (b, t) => t.Transform(b));

            this.Graph = builder;

            this.OnGraphChanged(new SpecimenBuilderNodeEventArgs(this.Graph));
        }
    }
}
