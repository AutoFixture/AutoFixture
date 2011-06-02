using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class GuardClauseAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            // Exercise system
            var sut = new GuardClauseAssertion();
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyPropertyDoesNotThrow()
        {
            // Fixture setup
            var sut = new GuardClauseAssertion();
            // Exercise system and verify outcome
            var property = typeof(SingleParameterType<object>).GetProperty("Parameter");
            Assert.DoesNotThrow(() =>
                sut.Verify(property));
            // Teardown
        }
    }
}
