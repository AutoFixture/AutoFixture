using System.Collections.Generic;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IValueGuardConvention
    {
        IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(Fixture fixture);
    }
}