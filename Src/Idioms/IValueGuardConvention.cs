using System.Collections.Generic;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IValueGuardConvention
    {
        IEnumerable<IInvalidValue> CreateInvalids(Fixture fixture);
    }
}