using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class SpecimenBuilderNodeEventArgs : EventArgs
    {
        private readonly ISpecimenBuilderNode graph;

        public SpecimenBuilderNodeEventArgs(ISpecimenBuilderNode graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");

            this.graph = graph;
        }

        public ISpecimenBuilderNode Graph
        {
            get { return this.graph; }
        }
    }
}
