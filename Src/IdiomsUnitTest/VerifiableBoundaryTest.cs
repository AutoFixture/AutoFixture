using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class VerifiableBoundaryTest
    {
        [Fact]
        public void VerifyBoundariesAgainstNullBoundaryThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                VerifiableBoundary.VerifyBoundaries(null));
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesInvokesSutWithCorrectConvention()
        {
            // Fixture setup
            var verified = false;
            var mock = new DelegatingVerifiableBoundary { OnVerifyBoundaries = c => verified = c is DefaultBoundaryConvention };
            // Exercise system
            mock.VerifyBoundaries();
            // Verify outcome
            Assert.True(verified, "Mock verified.");
            // Teardown
        }
    }
}
