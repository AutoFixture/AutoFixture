using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class TaggedNode : CompositeSpecimenBuilder
    {
        private readonly object tag;

        public TaggedNode(object tag, params ISpecimenBuilder[] builders)
            : base(builders)
        {
            this.tag = tag;
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new TaggedNode(this.tag, builders.ToArray());
        }

        public object Tag
        {
            get { return this.tag; }
        }
    }
}
