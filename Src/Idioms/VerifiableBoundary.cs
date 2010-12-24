using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public static class VerifiableBoundary
    {
        public static void VerifyBoundaryBehavior(this IVerifiableBoundary boundary)
        {
            if (boundary == null)
            {
                throw new ArgumentNullException("boundary");
            }

            boundary.VerifyBoundaryBehavior(new DefaultBoundaryConvention());
        }
    }
}
