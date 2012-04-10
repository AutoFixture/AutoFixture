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
            this.Tag = new object();
        }

        public MarkerNode(IEnumerable<ISpecimenBuilder> builders)
            : base(builders)
        {
            this.Tag = new object();
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new MarkerNode(builders);
        }

        public object Tag { get; set; }
    }
}
