using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ploeh.AutoFixture.Kernel
{
    public class OmitOnRecursionGuard : RecursionGuard
    {
        public OmitOnRecursionGuard(ISpecimenBuilder builder)
            : base(builder)
        {
        }

        public OmitOnRecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
            : base(builder, comparer)
        {
        }

        public override object HandleRecursiveRequest(object request)
        {
            return new OmitSpecimen();
        }

        public override ISpecimenBuilderNode Compose(
            IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = 
                CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new OmitOnRecursionGuard(composedBuilder, this.Comparer);
        }
    }
}
