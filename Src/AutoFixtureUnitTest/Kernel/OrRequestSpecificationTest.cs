using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class OrRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new OrRequestSpecification();
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullSpecificationsWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new OrRequestSpecification((IEnumerable<IRequestSpecification>)null));
            // Teardown
        }

        [Fact]
        public void SpecificationsMatchesConstructorParams()
        {
            // Fixture setup
            var expectedSpecifications = new[] { new DelegatingRequestSpecification(), new DelegatingRequestSpecification(), new DelegatingRequestSpecification() };
            var sut = new OrRequestSpecification(expectedSpecifications);
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
            var sut = new OrRequestSpecification(expectedSpecifications.Cast<IRequestSpecification>());
            // Exercise system
            IEnumerable<IRequestSpecification> result = sut.Specifications;
            // Verify outcome
            Assert.True(expectedSpecifications.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(true, new bool[0])]
        [InlineData(false, new[] { false })]
        [InlineData(true, new[] { true })]
        [InlineData(false, new[] { false, false })]
        [InlineData(true, new[] { true, false })]
        [InlineData(true, new[] { false, true })]
        [InlineData(true, new[] { true, true })]
        [InlineData(true, new[] { true, false, true })]
        [InlineData(true, new[] { true, true, true })]
        public void IsSatisfiedByReturnsCorrectResult(bool expectedResult, IEnumerable<bool> decoratedResults)
        {
            // Fixture setup
            var decoratedSpecs = from b in decoratedResults
                                 select new DelegatingRequestSpecification { OnIsSatisfiedBy = r => b };
            var sut = new OrRequestSpecification(decoratedSpecs.Cast<IRequestSpecification>());
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
            var sut = new OrRequestSpecification(specMock);
            // Exercise system
            sut.IsSatisfiedBy(expectedRequest);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }
    }
}
