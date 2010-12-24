using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class DefaultBoundaryConvention : CompositeBoundaryConvention
    {
        public DefaultBoundaryConvention()
            : base(
                new GuidBoundaryConvention(),
                new ValueTypeBoundaryConvention(),
                new StringBoundaryConvention(),
                new ReferenceTypeBoundaryConvention())
        {
        }
    }
}
