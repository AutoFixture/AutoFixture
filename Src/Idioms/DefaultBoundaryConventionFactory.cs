using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class DefaultBoundaryConventionFactory : CompositeBoundaryConventionFactory
    {
        public DefaultBoundaryConventionFactory()
            : base(
                new GuidBoundaryConventionFactory(),
                new ValueTypeBoundaryConventionFactory(),
                new StringBoundaryConventionFactory(),
                new ReferenceTypeBoundaryConventionFactory())
        {
        }
    }
}