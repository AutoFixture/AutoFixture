using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.TUnit.UnitTest.TestTypes;

public class EmptyClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}