using System.Collections.Generic;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IValueGuardConvention
    {
        IEnumerable<IBoundaryBehavior> CreateInvalids(Fixture fixture);
    }
}