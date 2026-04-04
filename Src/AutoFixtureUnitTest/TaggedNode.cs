using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest;

public class TaggedNode : CompositeSpecimenBuilder
{
    public TaggedNode(object tag, params ISpecimenBuilder[] builders)
        : base(builders)
    {
        Tag = tag;
    }

    public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
    {
        return new TaggedNode(Tag, builders.ToArray());
    }

    public object Tag { get; }
}