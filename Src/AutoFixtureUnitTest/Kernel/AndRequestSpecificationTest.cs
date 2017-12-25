using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class AndRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Arrange
            // Act
            var sut = new AndRequestSpecification();
            // Assert
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
        }

        [Fact]
        public void CreateWithNullSpecificationsWillThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new AndRequestSpecification((IEnumerable<IRequestSpecification>)null));
        }

        [Fact]
        public void SpecificationsMatchesConstructorParams()
        {
            // Arrange
            var expectedSpecifications = new[] { new DelegatingRequestSpecification(), new DelegatingRequestSpecification(), new DelegatingRequestSpecification() };
            var sut = new AndRequestSpecification(expectedSpecifications);
            // Act
            IEnumerable<IRequestSpecification> result = sut.Specifications;
            // Assert
            Assert.True(expectedSpecifications.SequenceEqual(result));
        }

        [Fact]
        public void SpecificationsMatchesConstructorSpecifications()
        {
            // Arrange
            var expectedSpecifications = new[] { new DelegatingRequestSpecification(), new DelegatingRequestSpecification(), new DelegatingRequestSpecification() };
            var sut = new AndRequestSpecification(expectedSpecifications.Cast<IRequestSpecification>());
            // Act
            IEnumerable<IRequestSpecification> result = sut.Specifications;
            // Assert
            Assert.True(expectedSpecifications.SequenceEqual(result));
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
            // Arrange
            var decoratedSpecs = from b in decoratedResults
                                 select new DelegatingRequestSpecification { OnIsSatisfiedBy = r => b };
            var sut = new AndRequestSpecification(decoratedSpecs.Cast<IRequestSpecification>());
            // Act
            var dummyRequest = new object();
            var result = sut.IsSatisfiedBy(dummyRequest);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsSatisfiedByPassesRequestToDecoratedSpecification()
        {
            // Arrange
            var expectedRequest = new object();
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest == r };
            var sut = new AndRequestSpecification(specMock);
            // Act
            sut.IsSatisfiedBy(expectedRequest);
            // Assert
            Assert.True(verified, "Mock verified");
        }
    }
}
