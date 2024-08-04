using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.Xunit2.UnitTest.TestTypes
{
    public class ClassWithNullTestCases : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return null;
            yield return null;
            yield return null;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}