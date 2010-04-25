using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class InclusiveRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new InclusiveRequestSpecification();
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsTrue()
        {
            // Fixture setup
            var sut = new InclusiveRequestSpecification();
            // Exercise system
            var dummyRequest = new object();
            var result = sut.IsSatisfiedBy(dummyRequest);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }
    }
}
