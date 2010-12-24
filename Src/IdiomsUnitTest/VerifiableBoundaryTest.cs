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
        public void VerifyBoundaryBehaviorAgainstNullBoundaryThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                VerifiableBoundary.VerifyBoundaryBehavior(null));
            // Teardown
        }

        [Fact]
        public void VerifyBoundaryBehaviorInvokesSutWithCorrectConvention()
        {
            // Fixture setup
            var verified = false;
            var mock = new DelegatingVerifiableBoundary { OnVerifyBoundaryBehavior = c => verified = c is DefaultBoundaryConvention };
            // Exercise system
            mock.VerifyBoundaryBehavior();
            // Verify outcome
            Assert.True(verified, "Mock verified.");
            // Teardown
        }
    }
}
