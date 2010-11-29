using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    public class DefaultBoundaryConventionFactoryCollection : Collection<IBoundaryConventionFactory>
    {
        public DefaultBoundaryConventionFactoryCollection() : base (
            new IBoundaryConventionFactory[] {
                new GuidBoundaryConventionFactory(), 
                new ValueTypeBoundaryConventionFactory(),
                new StringBoundaryConventionFactory(),
                new ReferenceTypeBoundaryConventionFactory()
            }.ToList())
        {
        }
    }
}