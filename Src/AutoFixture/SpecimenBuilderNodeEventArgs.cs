using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Event arguments concerning an <see cref="ISpecimenBuilderNode" />
    /// instance.
    /// </summary>
    public class SpecimenBuilderNodeEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SpecimenBuilderNodeEventArgs" /> class.
        /// </summary>
        /// <param name="graph">The graph associated with an event.</param>
        /// <exception cref="System.ArgumentNullException">graph.</exception>
        public SpecimenBuilderNodeEventArgs(ISpecimenBuilderNode graph)
        {
            this.Graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        /// <summary>
        /// Gets the graph associated with an event.
        /// </summary>
        public ISpecimenBuilderNode Graph { get; }
    }
}
