using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    internal class DelegatingTracingBuilder : TracingBuilder
    {
        public DelegatingTracingBuilder()
            : this(new DelegatingSpecimenBuilder())
        {
        }

        public DelegatingTracingBuilder(ISpecimenBuilder builder)
            : base(builder)
        {
        }

        internal void RaiseSpecimenCreated(SpecimenCreatedEventArgs e)
        {
            this.OnSpecimenCreated(e);
        }

        internal void RaiseSpecimenRequested(RequestTraceEventArgs e)
        {
            this.OnSpecimenRequested(e);
        }
    }
}
