using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ploeh.AutoFixture.Kernel
{
    public class OmitRecursionGuard : RecursionGuard
    {
        public OmitRecursionGuard(ISpecimenBuilder builder)
            : base(builder)
        {
        }

        public OmitRecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
            : base(builder, comparer)
        {
        }

        public override object HandleRecursiveRequest(object request)
        {
            return new OmitSpecimen();
        }
    }
}
