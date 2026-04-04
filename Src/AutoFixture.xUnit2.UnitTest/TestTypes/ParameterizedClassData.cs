using System.Collections;
using System.Collections.Generic;
using TestTypeFoundation;

namespace AutoFixture.Xunit2.UnitTest.TestTypes;

public class ParameterizedClassData : IEnumerable<object[]>
{
    private readonly int _p1;
    private readonly string _p2;
    private readonly EnumType _p3;

    public ParameterizedClassData(int p1, string p2, EnumType p3)
    {
        _p1 = p1;
        _p2 = p2;
        _p3 = p3;
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { _p1, _p2, _p3 };
        yield return new object[] { _p1, _p2, _p3 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}