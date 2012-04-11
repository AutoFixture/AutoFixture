using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class MarkerNode : CompositeSpecimenBuilder
    {
        public MarkerNode(params ISpecimenBuilder[] builders)
            : base(builders)
        {
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new MarkerNode(builders.ToArray());
        }
    }
}
