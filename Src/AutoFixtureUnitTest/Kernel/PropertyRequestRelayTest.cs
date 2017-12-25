using System;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class PropertyRequestRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new PropertyRequestRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new PropertyRequestRelay();
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
            var sut = new PropertyRequestRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateFromNonPropertyRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonParameterRequest = new object();
            var sut = new PropertyRequestRelay();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonParameterRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromPropertyRequestWillReturnCorrectResultWhenContainerCannotSatisfyRequest()
        {
            // Arrange
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            var container = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            var sut = new PropertyRequestRelay();
            // Act
            var result = sut.Create(propertyInfo, container);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromPropertyRequestWillReturnCorrectResultWhenContainerCanSatisfyRequest()
        {
            // Arrange
            var expectedSpecimen = new object();
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedSpecimen };
            var sut = new PropertyRequestRelay();
            // Act
            var result = sut.Create(propertyInfo, container);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }

        [Fact]
        public void CreateFromParameterRequestWillCorrectlyInvokeContainer()
        {
            // Arrange
            var sut = new PropertyRequestRelay();
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            var expectedRequest = new SeededRequest(propertyInfo.PropertyType, propertyInfo.Name);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContext();
            containerMock.OnResolve = r =>
            {
                Assert.Equal(expectedRequest, r);
                mockVerified = true;
                return null;
            };
            // Act
            sut.Create(propertyInfo, containerMock);
            // Assert
            Assert.True(mockVerified, "Mock verification");
        }
    }
}
