using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    public class DelegatingTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> data;

        public DelegatingTestData(params object[][] data)
        {
            this.data = data.ToList();
        }

        public DelegatingTestData(IEnumerable<object[]> data)
        {
            this.data = data as List<object[]> ?? data.ToList();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}