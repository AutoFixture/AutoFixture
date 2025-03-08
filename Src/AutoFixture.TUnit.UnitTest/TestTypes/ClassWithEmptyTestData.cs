using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.TUnit.UnitTest.TestTypes;

public class ClassWithEmptyTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [];
        yield return [];
        yield return [];
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}