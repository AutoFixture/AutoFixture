using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class SpecimenCreatedEventArgs : SpecimenTraceEventArgs
    {
        private readonly object specimen;

        public SpecimenCreatedEventArgs(object request, object specimen, int depth)
            : base(request, depth)
        {
            this.specimen = specimen;
        }

        public object Specimen
        {
            get { return this.specimen; }
        }
    }
}
