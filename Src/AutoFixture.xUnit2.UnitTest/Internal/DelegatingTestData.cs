using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.Xunit2.UnitTest.Internal
{
    public class DelegatingTestData : IEnumerable<object[]>
    {
        private readonly IReadOnlyList<object[]> arguments;

        public DelegatingTestData(params object[][] arguments)
        {
            this.arguments = arguments;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return this.arguments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}