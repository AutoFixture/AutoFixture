using System.Collections;
using System.Collections.Generic;
using TestTypeFoundation;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    public class ParameterizedClassData : IEnumerable<object[]>
    {
        private readonly int p1;
        private readonly string p2;
        private readonly EnumType p3;

        public ParameterizedClassData(int p1, string p2, EnumType p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { this.p1, this.p2, this.p3 };
            yield return new object[] { this.p1, this.p2, this.p3 };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}