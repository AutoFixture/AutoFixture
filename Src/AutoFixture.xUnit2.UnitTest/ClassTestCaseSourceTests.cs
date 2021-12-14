using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest
{
    public class ClassTestCaseSourceTests
    {
        [Fact]
        public void Foo()
        {
            object[][] data = new[]
            {
                new object[] { 1, 2.3m, "test-value-000", new PropertyHolder<float> { Property = 3.2f } },
                new object[] { 5, 23.393m, "test-value-001", new PropertyHolder<float> { Property = 19.7f } }
            };

            ClassTestCaseSource sut = new ClassTestCaseSource(
                typeof(DelegatingTestData),
                new object[] { data.AsEnumerable() });

            Assert.Equal(data, sut.Cast<object[]>());
        }
    }

    public class RecorgingTestClass
    {
        public void Foo(int a, decimal b, string c, PropertyHolder<float> d)
        {
        }
    }

    public class DelegatingTestData : IEnumerable<object[]>
    {
        private readonly IEnumerable<object[]> data;

        public DelegatingTestData(IEnumerable<object[]> data)
        {
            this.data = data;
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

    internal static class EmptyArray<T>
    {
        public static readonly T[] Value = new T[0];
    }
}
