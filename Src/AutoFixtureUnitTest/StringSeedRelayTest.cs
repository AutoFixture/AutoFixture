using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class StringSeedRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new StringSeedRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Arrange
            var sut = new StringSeedRelay();
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
            var sut = new StringSeedRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithNonSeedWillReturnNull()
        {
            // Arrange
            var sut = new StringSeedRelay();
            var nonSeed = new object();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonSeed, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithNonStringRequestSeedWillReturnNull()
        {
            // Arrange
            var sut = new StringSeedRelay();
            var nonStringRequestSeed = new SeededRequest(typeof(object), "Anonymous value");
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonStringRequestSeed, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithNonStringSeedWillReturnCorrectResult()
        {
            // Arrange
            var sut = new StringSeedRelay();
            var nonStringSeed = new SeededRequest(typeof(string), new object());
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonStringSeed, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithStringSeedWhenContainerCannotCreateStringsWillReturnCorrectResult()
        {
            // Arrange
            var sut = new StringSeedRelay();
            var stringSeed = new SeededRequest(typeof(string), "Anonymous value");
            var unableContainer = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            // Act
            var result = sut.Create(stringSeed, unableContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithStringSeedWhenContainerCanCreateStringsWillReturnCorrectResult()
        {
            // Arrange
            var seedString = Guid.NewGuid().ToString();
            var containerString = Guid.NewGuid().ToString();

            var sut = new StringSeedRelay();
            var stringSeed = new SeededRequest(typeof(string), seedString);
            var container = new DelegatingSpecimenContext { OnResolve = r => containerString };
            // Act
            var result = sut.Create(stringSeed, container);
            // Assert
            var expectedString = seedString + containerString;
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void CreateWithStringSeedWillCorrectlyInvokeContainer()
        {
            // Arrange
            var sut = new StringSeedRelay();
            var stringSeed = new SeededRequest(typeof(string), "Anonymous value");

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContext();
            containerMock.OnResolve = r =>
                {
                    Assert.Equal(typeof(string), r);
                    mockVerified = true;
                    return null;
                };
            // Act
            sut.Create(stringSeed, containerMock);
            // Assert
            Assert.True(mockVerified, "Mock verification");
        }
    }
}
