using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.Xunit2.UnitTest.TestTypes;

public class DelegatingTestData : IEnumerable<object[]>
{
    private readonly List<object[]> _data;

    public DelegatingTestData(params object[][] data)
    {
        _data = data.ToList();
    }

    public DelegatingTestData(IEnumerable<object[]> data)
    {
        _data = data as List<object[]> ?? data.ToList();
    }

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}