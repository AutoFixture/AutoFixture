using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class MarkerNode : CompositeSpecimenBuilder
    {
        public readonly static IEqualityComparer<ISpecimenBuilder> Comparer = new TagComparer();

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

        private class TagComparer : IEqualityComparer<ISpecimenBuilder>
        {
            public bool Equals(ISpecimenBuilder x, ISpecimenBuilder y)
            {
                var n1 = x as MarkerNode;
                var n2 = y as MarkerNode;
                if (n1 != null && n2 != null)
                    return n1.Tag.Equals(n2.Tag);
                return x.Equals(y);
            }

            public int GetHashCode(ISpecimenBuilder obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
