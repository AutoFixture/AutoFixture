using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel;

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
        OnSpecimenCreated(e);
    }

    internal void RaiseSpecimenRequested(RequestTraceEventArgs e)
    {
        OnSpecimenRequested(e);
    }
}