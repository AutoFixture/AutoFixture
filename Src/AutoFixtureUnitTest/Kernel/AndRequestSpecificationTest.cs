using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class AndRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new AndRequestSpecification();
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullSpecificationsWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new AndRequestSpecification((IEnumerable<IRequestSpecification>)null));
            // Teardown
        }

        [Fact]
        public void SpecificationsMatchesConstructorParams()
        {
            // Fixture setup
            var expectedSpecifications = new[] { new DelegatingRequestSpecification(), new DelegatingRequestSpecification(), new DelegatingRequestSpecification() };
            var sut = new AndRequestSpecification(expectedSpecifications);
            // Exercise system
            IEnumerable<IRequestSpecification> result = sut.Specifications;
            // Verify outcome
            Assert.True(expectedSpecifications.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void SpecificationsMatchesConstructorSpecifications()
        {
            // Fixture setup
            var expectedSpecifications = new[] { new DelegatingRequestSpecification(), new DelegatingRequestSpecification(), new DelegatingRequestSpecification() };
            var sut = new AndRequestSpecification(expectedSpecifications.Cast<IRequestSpecification>());
            // Exercise system
            IEnumerable<IRequestSpecification> result = sut.Specifications;
            // Verify outcome
            Assert.True(expectedSpecifications.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(false, new bool[0])]
        [InlineData(false, new[] { false })]
        [InlineData(true, new[] { true })]
        [InlineData(false, new[] { false, false })]
        [InlineData(false, new[] { true, false })]
        [InlineData(false, new[] { false, true })]
        [InlineData(true, new[] { true, true })]
        [InlineData(false, new[] { true, false, true })]
        [InlineData(true, new[] { true, true, true })]
        public void IsSatisfiedByReturnsCorrectResult(bool expectedResult, IEnumerable<bool> decoratedResults)
        {
            // Fixture setup
            var decoratedSpecs = from b in decoratedResults
                                 select new DelegatingRequestSpecification { OnIsSatisfiedBy = r => b };
            var sut = new AndRequestSpecification(decoratedSpecs.Cast<IRequestSpecification>());
            // Exercise system
            var dummyRequest = new object();
            var result = sut.IsSatisfiedBy(dummyRequest);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByPassesRequestToDecoratedSpecification()
        {
            // Fixture setup
            var expectedRequest = new object();
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest == r };
            var sut = new AndRequestSpecification(specMock);
            // Exercise system
            sut.IsSatisfiedBy(expectedRequest);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }
    }
}
