using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Presents a horizontal slice of an <see cref="ISpecimenBuilderNode" />
    /// graph as if it was a collection.
    /// </summary>
    /// <remarks>
    /// This class encapsulates an <see cref="ISpecimenBuilderNode" /> graph
    /// and presents a horizontal slice of the graph as if it was a collection.
    /// For all mutating operations on the collection, the
    /// SpecimenBuilderNodeCollectionAdapter replaces its internal graph with a
    /// new graph that encapsulates the new state. The original graph is not
    /// mutated, but when a seemingly mutating method is invoked, the
    /// <see cref="GraphChanged" /> event is raised.
    /// </remarks>
    public class SpecimenBuilderNodeAdapterCollection : IList<ISpecimenBuilder>
    {
        private readonly Func<ISpecimenBuilderNode, bool> isAdaptedBuilder;
        private ISpecimenBuilderNode adaptedBuilderNode;
        private ISpecimenBuilderNode graph;

        private ISpecimenBuilderNode AdaptedBuilderNode
        {
            get
            {
                // We evaluate this value lazily for the performance reasons.
                // Usually during the Fixture construction it's customized a lot of times.
                // Some of the collections are not even touched after the construction and are immediately
                // recreated again after a subsequent customization.
                // To cover such scenarios we evaluate value on demand only saving the initialization time.
                if (this.adaptedBuilderNode != null)
                    return this.adaptedBuilderNode;

                // The intermediate "result" variable is needed to ensure that null value can be never returned
                // in case of concurrency (we can set field to null). While current collection implementation doesn't
                // seem to support concurrency, the additional guard adds more safety.
                var result = this.adaptedBuilderNode = this.FindAdaptedSpecimenBuilderNode();
                return result;
            }
        }

        private void InvalidateCachedAdaptedBuilderNode() => this.adaptedBuilderNode = null;

        private IEnumerable<ISpecimenBuilder> AdaptedBuilders => this.AdaptedBuilderNode;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SpecimenBuilderNodeAdapterCollection" /> class.
        /// </summary>
        /// <param name="graph">The underlying graph.</param>
        /// <param name="adaptedBuilderPredicate">
        /// A predicate which is used to identify a node in
        /// <paramref name="graph" />.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="adaptedBuilderPredicate" /> must identify a single
        /// node in <paramref name="graph" />. If zero or several nodes are
        /// identified by this predicate, an exception will be thrown.
        /// </para>
        /// <para>
        /// The node identified by the predicate is used as a source of the
        /// collection exposed by the SpecimenBuilderNodeCollectionAdapter
        /// class. When you enumerate all items in the collection, you really
        /// enumerate the children of the identified node.
        /// </para>
        /// </remarks>
        public SpecimenBuilderNodeAdapterCollection(
            ISpecimenBuilderNode graph,
            Func<ISpecimenBuilderNode, bool> adaptedBuilderPredicate)
        {
            this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
            this.isAdaptedBuilder = adaptedBuilderPredicate ?? throw new ArgumentNullException(nameof(adaptedBuilderPredicate));
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

        /// <summary>
        /// Determines the index of a specific item in the collection.
        /// </summary>
        /// <param name="item">The object to locate in the collection.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list;
        /// otherwise, -1.
        /// </returns>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public int IndexOf(ISpecimenBuilder item)
        {
            return this.AdaptedBuilders.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the collection at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item" /> should be
        /// inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert into the collection.
        /// </param>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// <para>
        /// Since this is a mutating operation it raises the
        /// <see cref="GraphChanged" /> event.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public void Insert(int index, ISpecimenBuilder item)
        {
            this.Mutate(this.AdaptedBuilders.Insert(index, item));
        }

        /// <summary>
        /// Removes the item at the specified index from the collection.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// <para>
        /// Since this is a mutating operation it raises the
        /// <see cref="GraphChanged" /> event.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public void RemoveAt(int index)
        {
            this.Mutate(this.AdaptedBuilders.RemoveAt(index));
        }

        /// <summary>
        /// Gets or sets the <see cref="ISpecimenBuilder" /> at the specified
        /// index.
        /// </summary>
        /// <value>
        /// The <see cref="ISpecimenBuilder" /> at the specified index.
        /// </value>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        /// <para>
        /// Since setting the value is a mutating operation it raises the
        /// <see cref="GraphChanged" /> event.
        /// </para>
        /// <returns>
        /// The <see cref="ISpecimenBuilder" /> at the specified index.
        /// </returns>
        public ISpecimenBuilder this[int index]
        {
            get => this.AdaptedBuilders.ElementAt(index);
            set => this.Mutate(this.AdaptedBuilders.SetItem(index, value));
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">he object to add to the collection.</param>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// <para>
        /// Since this is a mutating operation it raises the
        /// <see cref="GraphChanged" /> event.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public void Add(ISpecimenBuilder item)
        {
            this.Mutate(this.AdaptedBuilders.Concat(new[] { item }));
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// <para>
        /// Since this is a mutating operation it raises the
        /// <see cref="GraphChanged" /> event.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public void Clear()
        {
            this.Mutate(Enumerable.Empty<ISpecimenBuilder>());
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">The item to locate in the collection.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="item"/> is found in the
        /// collection; otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public bool Contains(ISpecimenBuilder item)
        {
            return this.AdaptedBuilders.Contains(item);
        }

        /// <summary>Copies the elements of the collection to an
        /// <see cref="Array" />, starting at a particular Array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array" /> that is the destination of
        /// the elements copied from the collection. The Array must have
        /// zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public void CopyTo(ISpecimenBuilder[] array, int arrayIndex)
        {
            this.AdaptedBuilders.ToArray().CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        /// <value>The number of elements contained in the collection.</value>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public int Count
        {
            get { return this.AdaptedBuilders.Count(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this instance is read only; otherwise,
        /// <see langword="false" />.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the
        /// collection.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the collection.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="item" /> was
        /// successfully removed from the collection; otherwise,
        /// <see langword="false" />. This method also returns
        /// <see langword="false" /> if the item is not found in the original
        /// collection.
        /// </returns>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// <para>
        /// Since this is a mutating operation it raises the
        /// <see cref="GraphChanged" /> event.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public bool Remove(ISpecimenBuilder item)
        {
            var contained = this.Contains(item);

            var index = this.IndexOf(item);
            this.RemoveAt(index);

            return contained;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{ISpecimenBuilder}" /> that can be used to
        /// iterate through the collection.
        /// </returns>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            return this.AdaptedBuilders.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator" /> object that can
        /// be used to iterate through the collection.
        /// </returns>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Gets the graph which is the backing store of the collection.
        /// </summary>
        /// <value>
        /// The graph which is the backing store of the collection.
        /// </value>
        /// <remarks>
        /// <para>
        /// In this context, the term <i>collection</i> refers to the
        /// horizontal slice through the encapsulated
        /// <see cref="ISpecimenBuilderNode" /> graph that this instance
        /// exposes as a collection.
        /// </para>
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection" />
        /// <seealso cref="SpecimenBuilderNodeAdapterCollection(ISpecimenBuilderNode, Func{ISpecimenBuilderNode, bool})" />
        public ISpecimenBuilderNode Graph
        {
            get => this.graph;
            private set
            {
                this.graph = value;
                this.InvalidateCachedAdaptedBuilderNode();
                this.OnGraphChanged(new SpecimenBuilderNodeEventArgs(value));
            }
        }

        /// <summary>Raises the <see cref="GraphChanged" /> event.</summary>
        /// <param name="e">
        /// The <see cref="SpecimenBuilderNodeEventArgs" /> instance containing
        /// the event data.
        /// </param>
        protected virtual void OnGraphChanged(SpecimenBuilderNodeEventArgs e)
        {
            this.GraphChanged?.Invoke(this, e);
        }

        private void Mutate(IEnumerable<ISpecimenBuilder> builders)
        {
            var adaptedNode = this.AdaptedBuilderNode;

            this.Graph = this.Graph.ReplaceNodes(
                with: builders,
                when: adaptedNode.Equals);
        }

        private ISpecimenBuilderNode FindAdaptedSpecimenBuilderNode()
        {
            var markerNode = this.Graph.FindFirstNode(this.isAdaptedBuilder);
            return (ISpecimenBuilderNode)markerNode.First();
        }
    }
}
