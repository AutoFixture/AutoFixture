using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class TaggedNodeComparer : IEqualityComparer<ISpecimenBuilder>
    {
        private readonly IEqualityComparer<ISpecimenBuilder> nonTaggedComparer;

        public TaggedNodeComparer()
            : this(EqualityComparer<ISpecimenBuilder>.Default)
        {
        }

        public TaggedNodeComparer(IEqualityComparer<ISpecimenBuilder> nonTaggedComparer)
        {
            this.nonTaggedComparer = nonTaggedComparer;
        }

        public bool Equals(ISpecimenBuilder x, ISpecimenBuilder y)
        {
            var n1 = x as TaggedNode;
            var n2 = y as TaggedNode;
            if (n1 != null && n2 != null)
                return n1.Tag.Equals(n2.Tag);
            return this.nonTaggedComparer.Equals(x, y);
        }

        public int GetHashCode(ISpecimenBuilder obj)
        {
            var tn = obj as TaggedNode;
            if (tn != null)
                return tn.Tag.GetHashCode();
            return this.nonTaggedComparer.GetHashCode(obj);
        }
    }
}
