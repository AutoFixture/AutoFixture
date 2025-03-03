using System;
using System.Collections;
using System.Collections.Generic;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    public class MixedTypeClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return [];
            yield return [9];
            yield return [12, "test-12"];
            yield return [223, "test-17", EnumType.Third];
            yield return [-95, "test-92", EnumType.Second, new Tuple<string, int>("myValue", 5)];
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
