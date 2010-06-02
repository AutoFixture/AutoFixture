using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ManyRequestTest
    {
        [Fact]
        public void InitializeWithNullRequestThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ManyRequest(null));
            // Teardown
        }

        [Fact]
        public void RequestIsCorrect()
        {
            // Fixture setup
            var expectedRequest = new object();
            var sut = new ManyRequest(expectedRequest);
            // Exercise system
            var result = sut.Request;
            // Verify outcome
            Assert.Equal(expectedRequest, result);
            // Teardown
        }
    }
}
