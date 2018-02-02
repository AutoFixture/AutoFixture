using System.Collections.Generic;

namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public interface IDerivedFromEnumerableInterface<out T>: IEnumerable<T>
    {
    }
}