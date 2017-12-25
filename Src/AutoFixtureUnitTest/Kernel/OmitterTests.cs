using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class OmitterTests
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new Omitter();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestThrows()
        {
            // Arrange
            var sut = new Omitter();
            // Act & assert
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(null, dummyContext));
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Arrange
            var sut = new Omitter();
            // Act
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(dummyRequest, dummyContext);
            // Assert
            Assert.IsAssignableFrom<OmitSpecimen>(actual);
        }

        [Fact]
        public void SpecificationMatchesConstructorArgument()
        {
            // Arrange
            var expected = new DelegatingRequestSpecification();
            var sut = new Omitter(expected);
            // Act
            IRequestSpecification actual = sut.Specification;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithNullSpecificationThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new Omitter(null));
        }

        [Fact]
        public void CreateWhenSpecificationIsFalseReturnsCorrectResult()
        {
            // Arrange
            var sut = new Omitter(new FalseRequestSpecification());
            var request = new object();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateWhenSpecificationMatchesRequestReturnsCorrectResult()
        {
            // Arrange
            var request = new object();
            var specification = new DelegatingRequestSpecification
            {
                OnIsSatisfiedBy = request.Equals
            };
            var sut = new Omitter(specification);
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Assert
            Assert.IsAssignableFrom<OmitSpecimen>(actual);
        }
    }
}
