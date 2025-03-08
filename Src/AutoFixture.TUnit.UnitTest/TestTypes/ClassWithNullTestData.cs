using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.TUnit.UnitTest.TestTypes;

public class ClassWithNullTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return null;
        yield return null;
        yield return null;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}