using System;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class FieldRequestRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new FieldRequestRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new FieldRequestRelay();
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
            var sut = new FieldRequestRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateFromNonFieldRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonFieldRequest = new object();
            var sut = new FieldRequestRelay();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonFieldRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromFieldRequestWillReturnCorrectResultWhenContainerCannotSatisfyRequest()
        {
            // Arrange
            var fieldInfo = typeof(FieldHolder<object>).GetField("Field");
            var container = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            var sut = new FieldRequestRelay();
            // Act
            var result = sut.Create(fieldInfo, container);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromFieldRequestWillReturnCorrectResultWhenContainerCanSatisfyRequest()
        {
            // Arrange
            var expectedSpecimen = new object();
            var fieldInfo = typeof(FieldHolder<object>).GetField("Field");
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedSpecimen };
            var sut = new FieldRequestRelay();
            // Act
            var result = sut.Create(fieldInfo, container);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }

        [Fact]
        public void CreateFromFieldRequestWillCorrectlyInvokeContainer()
        {
            // Arrange
            var sut = new FieldRequestRelay();
            var fieldInfo = typeof(FieldHolder<object>).GetField("Field");
            var expectedRequest = new SeededRequest(fieldInfo.FieldType, fieldInfo.Name);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContext();
            containerMock.OnResolve = r => mockVerified = expectedRequest.Equals(r);
            // Act
            sut.Create(fieldInfo, containerMock);
            // Assert
            Assert.True(mockVerified, "Mock verification");
        }
    }
}
