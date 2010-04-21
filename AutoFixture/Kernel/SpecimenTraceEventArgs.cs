using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class SpecimenTraceEventArgs : EventArgs
    {
        private readonly object request;
        private readonly int depth;

        public SpecimenTraceEventArgs(object request, int depth)
        {        
            this.request = request;
            this.depth = depth;
        }

        public int Depth
        {
            get { return this.depth; }
        }

        public object Request
        {
            get { return this.request; }
        }
    }
}
