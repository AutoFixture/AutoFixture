using System;
using System.Collections;
using System.Collections.Generic;
using TestTypeFoundation;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    public class MixedTypeClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return Array.Empty<object>();
            yield return new object[] { 9 };
            yield return new object[] { 12, "test-12" };
            yield return new object[] { 223, "test-17", EnumType.Third };
            yield return new object[] { -95, "test-92", EnumType.Second, new Tuple<string, int>("myValue", 5) };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
