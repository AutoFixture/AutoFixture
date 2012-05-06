using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class BehaviorRoot : CompositeSpecimenBuilder
    {
        public BehaviorRoot(params ISpecimenBuilder[] builders)
            : base(builders)
        {
        }

        public BehaviorRoot(IEnumerable<ISpecimenBuilder> builders)
            : base(builders)
        {
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new BehaviorRoot(builders);
        }
    }
}
