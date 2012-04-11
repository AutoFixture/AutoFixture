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

        public readonly static IEqualityComparer<ISpecimenBuilder> Comparer = new TagComparer();

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

        private class TagComparer : IEqualityComparer<ISpecimenBuilder>
        {
            public bool Equals(ISpecimenBuilder x, ISpecimenBuilder y)
            {
                var n1 = x as TaggedNode;
                var n2 = y as TaggedNode;
                if (n1 != null && n2 != null)
                    return n1.Tag.Equals(n2.Tag);
                return x.Equals(y);
            }

            public int GetHashCode(ISpecimenBuilder obj)
            {
                var tn = obj as TaggedNode;
                if (tn != null)
                    return tn.tag.GetHashCode();
                return obj.GetHashCode();
            }
        }
    }
}
