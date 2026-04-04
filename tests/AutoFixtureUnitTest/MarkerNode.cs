using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest;

public class MarkerNode : ISpecimenBuilderNode
{
    private readonly ISpecimenBuilder _builder;

    public MarkerNode(ISpecimenBuilder builder)
    {
        _builder = builder;
    }

    public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
    {
        var l = builders.ToList();
        if (l.Count == 1)
            return new MarkerNode(l.Single());
        return new MarkerNode(new CompositeSpecimenBuilder(builders));
    }

    public object Create(object request, ISpecimenContext context)
    {
        return _builder.Create(request, context);
    }

    public IEnumerator<ISpecimenBuilder> GetEnumerator()
    {
        yield return _builder;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}