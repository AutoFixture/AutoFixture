using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.Xunit2.UnitTest.TestTypes
{
    public class EmptyClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}