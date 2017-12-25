using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SeedIgnoringRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new SeedIgnoringRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Arrange
            var sut = new SeedIgnoringRelay();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithNullContainerWillThrow()
        {
            // Arrange
            var sut = new SeedIgnoringRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateFromSeedWhenContainerCannotSatisfyWrappedRequestWillReturnNull()
        {
            // Arrange
            var anonymousSeed = new SeededRequest(typeof(object), new object());
            var unableContainer = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            var sut = new SeedIgnoringRelay();
            // Act
            var result = sut.Create(anonymousSeed, unableContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromSeedWhenContainerCanSatisfyWrappedRequestWillReturnCorrectResult()
        {
            // Arrange
            var anonymousSeed = new SeededRequest(typeof(object), new object());

            var expectedResult = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedResult };

            var sut = new SeedIgnoringRelay();
            // Act
            var result = sut.Create(anonymousSeed, container);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromSeedWillCorrectlyInvokeContainer()
        {
            // Arrange
            var sut = new SeedIgnoringRelay();
            var seededRequest = new SeededRequest(typeof(int), 1);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContext();
            containerMock.OnResolve = r =>
            {
                Assert.Equal(typeof(int), r);
                mockVerified = true;
                return null;
            };
            // Act
            sut.Create(seededRequest, containerMock);
            // Assert
            Assert.True(mockVerified, "Mock verification");
        }
    }
}
