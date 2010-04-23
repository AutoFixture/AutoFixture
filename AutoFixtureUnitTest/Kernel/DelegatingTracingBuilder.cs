using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    internal class DelegatingTracingBuilder : RequestTracker
    {
        public DelegatingTracingBuilder()
            : this(new DelegatingSpecimenBuilder())
        {
        }

        public DelegatingTracingBuilder(ISpecimenBuilder builder)
            : base(builder)
        {
            this.OnTrackRequest = r => { };
            this.OnTrackCreatedSpecimen = r => { };
        }

        protected override void TrackRequest(object request)
        {
            this.OnTrackRequest(request);
        }

        protected override void TrackCreatedSpecimen(object specimen)
        {
            this.OnTrackCreatedSpecimen(specimen);
        }

        internal void RaiseSpecimenCreated(SpecimenCreatedEventArgs e)
        {
            this.OnSpecimenCreated(e);
        }

        internal void RaiseSpecimenRequested(SpecimenTraceEventArgs e)
        {
            this.OnSpecimenRequested(e);
        }

        internal Action<object> OnTrackRequest { get; set; }
        internal Action<object> OnTrackCreatedSpecimen { get; set; }
        internal Func<object, ISpecimenContainer, object> OnCreate { get; set; }
    }
}
