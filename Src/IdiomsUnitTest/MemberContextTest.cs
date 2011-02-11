using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class MemberContextTest
    {
        [Fact]
        public void VerifyBoundariesAgainstNullBoundaryThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                MemberContext.VerifyBoundaries(null));
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesInvokesSutWithCorrectConvention()
        {
            // Fixture setup
            var verified = false;
            var mock = new DelegatingMemberContext { OnVerifyBoundaries = c => verified = c is DefaultBoundaryConvention };
            // Exercise system
            mock.VerifyBoundaries();
            // Verify outcome
            Assert.True(verified, "Mock verified.");
            // Teardown
        }
    }
}
