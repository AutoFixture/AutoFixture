using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ParameterRequestRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new ParameterRequestRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new ParameterRequestRelay();
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
            var sut = new ParameterRequestRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateFromNonParameterRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonParameterRequest = new object();
            var sut = new ParameterRequestRelay();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonParameterRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromParameterRequestWillReturnNullWhenContainerCannotSatisfyRequest()
        {
            // Arrange
            var parameterInfo = typeof(SingleParameterType<string>).GetTypeInfo().GetConstructors().First().GetParameters().First();
            var container = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            var sut = new ParameterRequestRelay();
            // Act
            var result = sut.Create(parameterInfo, container);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromParameterRequestWillReturnCorrectResultWhenContainerCanSatisfyRequest()
        {
            // Arrange
            var expectedSpecimen = new object();
            var parameterInfo = typeof(SingleParameterType<string>).GetTypeInfo().GetConstructors().First().GetParameters().First();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedSpecimen };
            var sut = new ParameterRequestRelay();
            // Act
            var result = sut.Create(parameterInfo, container);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }

        [Fact]
        public void CreateFromParameterRequestWillCorrectlyInvokeContainer()
        {
            // Arrange
            var sut = new ParameterRequestRelay();
            var parameterInfo = typeof(SingleParameterType<string>).GetTypeInfo().GetConstructors().First().GetParameters().First();
            var expectedRequest = new SeededRequest(parameterInfo.ParameterType, parameterInfo.Name);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContext();
            containerMock.OnResolve = r =>
            {
                Assert.Equal(expectedRequest, r);
                mockVerified = true;
                return null;
            };
            // Act
            sut.Create(parameterInfo, containerMock);
            // Assert
            Assert.True(mockVerified, "Mock verification");
        }
    }
}
