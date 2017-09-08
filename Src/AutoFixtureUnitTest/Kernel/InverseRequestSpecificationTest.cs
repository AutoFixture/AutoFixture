using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class InverseRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            var dummySpec = new DelegatingRequestSpecification();
            // Exercise system
            var sut = new InverseRequestSpecification(dummySpec);
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new InverseRequestSpecification(null));
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectly()
        {
            // Fixture setup
            var expectedSpec = new DelegatingRequestSpecification();
            var sut = new InverseRequestSpecification(expectedSpec);
            // Exercise system
            IRequestSpecification result = sut.Specification;
            // Verify outcome
            Assert.Equal(expectedSpec, result);
            // Teardown
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IsSatisfiedByReturnsCorrectResult(bool decoratedResult)
        {
            // Fixture setup
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => decoratedResult };
            var sut = new InverseRequestSpecification(spec);
            // Exercise system
            var dummyRequest = new object();
            var result = sut.IsSatisfiedBy(dummyRequest);
            // Verify outcome
            Assert.Equal(!decoratedResult, result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByInvokesDecoratedSpecWithCorrectRequest()
        {
            // Fixture setup
            var expectedRequest = new object();
            var verified = false;
            var mock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest == r };
            var sut = new InverseRequestSpecification(mock);
            // Exercise system
            sut.IsSatisfiedBy(expectedRequest);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }
    }
}
