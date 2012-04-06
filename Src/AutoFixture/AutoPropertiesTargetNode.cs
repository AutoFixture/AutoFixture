using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class AutoPropertiesTargetNode : CompositeSpecimenBuilder
    {
        public AutoPropertiesTargetNode(params ISpecimenBuilder[] builders)
            : base(builders)
        {
        }

        public AutoPropertiesTargetNode(IEnumerable<ISpecimenBuilder> builders)
            : base(builders)
        {
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new AutoPropertiesTargetNode(builders);
        }
    }
}
