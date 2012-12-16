using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class EnumerableEnumeratorTest
    {
        [Fact]
        public void GetEnumerator_ReturnsTheSpecifiedEnumerator()
        {
            // Fixture setup
            var expected = Enumerable.Empty<object>().GetEnumerator();
            var sut = new EnumerableEnumerator<object>(expected);

            // Exercise system
            var actual = sut.GetEnumerator();

            // Verify outcome
            Assert.Same(expected, actual);
        }

        [Fact]
        public void InitializeWithNullEnumeratorThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new EnumerableEnumerator<object>(null));
        }

        [Fact]
        public void SutIsGenericIEnumerable()
        {
            // Exercise system
            var sut = new EnumerableEnumerator<object>(Enumerable.Empty<object>().GetEnumerator());

            // Verify outcome
            Assert.IsAssignableFrom<IEnumerable<object>>(sut);
        }
    }
}
