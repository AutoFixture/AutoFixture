using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class WritablePropertyAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            // Exercise system
            var sut = new WritablePropertyAssertion();
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyPropertyDoesNotThrow()
        {
            // Fixture setup
            var sut = new WritablePropertyAssertion();
            var propertyInfo = typeof(ReadOnlyPropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(propertyInfo));
            // Teardown
        }
    }
}
