using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.Xunit.v3.UnitTest.TestTypes
{
    public class NullClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() => null;

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}