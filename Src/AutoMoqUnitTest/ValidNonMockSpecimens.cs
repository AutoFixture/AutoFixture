using System.Collections;
using System.Collections.Generic;
using AutoFixture.Kernel;

namespace AutoFixture.AutoMoq.UnitTest
{
    internal class ValidNonMockSpecimens : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {new NoSpecimen()};
            yield return new object[] {new OmitSpecimen()};
            yield return new object[] {null};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
