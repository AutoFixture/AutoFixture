using System.Collections.Generic;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IBoundaryConvention
    {
        IEnumerable<IBoundaryBehavior> CreateBoundaryBehaviors(IFixture fixture);
    }
}