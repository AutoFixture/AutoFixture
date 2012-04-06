using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class ResidueCollectorNode : CompositeSpecimenBuilder
    {
        public ResidueCollectorNode(params ISpecimenBuilder[] builders)
            : base(builders)
        {
        }

        public ResidueCollectorNode(IEnumerable<ISpecimenBuilder> builders)
            : base(builders)
        {
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new ResidueCollectorNode(builders);
        }
    }
}
