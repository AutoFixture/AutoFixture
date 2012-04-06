using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class AutoPropertiesTarget : CompositeSpecimenBuilder
    {
        public AutoPropertiesTarget(params ISpecimenBuilder[] builders)
            : base(builders)
        {
        }

        public AutoPropertiesTarget(IEnumerable<ISpecimenBuilder> builders)
            : base(builders)
        {
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            return new AutoPropertiesTarget(builders);
        }
    }
}
