using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
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
        /// <exception cref="System.ArgumentNullException">graph</exception>
        public SpecimenBuilderNodeEventArgs(ISpecimenBuilderNode graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            this.Graph = graph;
        }

        /// <summary>
        /// Gets the graph associated with an event.
        /// </summary>
        public ISpecimenBuilderNode Graph { get; }
    }
}
