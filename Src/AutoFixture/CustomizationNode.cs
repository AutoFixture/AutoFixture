using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class CustomizationNode : CompositeSpecimenBuilder
    {
        public CustomizationNode(params ISpecimenBuilder[] builders)
            : base(builders)
        {
        }

        public CustomizationNode(IEnumerable<ISpecimenBuilder> builders)
            : base(builders)
        {
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new CustomizationNode(builders);
        }
    }
}
