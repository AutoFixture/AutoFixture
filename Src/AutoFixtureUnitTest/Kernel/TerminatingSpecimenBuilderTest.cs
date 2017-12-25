using System;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class TerminatingSpecimenBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new TerminatingSpecimenBuilder();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateThrowsCorrectException()
        {
            // Arrange
            var sut = new TerminatingSpecimenBuilder();
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Act & assert
            Assert.Throws<ObjectCreationException>(() =>
                sut.Create(dummyRequest, dummyContext));
        }

        [Fact]
        public void ExceptionContainsInformationAboutRequest()
        {
            // Arrange
            var sut = new TerminatingSpecimenBuilder();
            var request = Guid.NewGuid();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var e = Assert.Throws<ObjectCreationException>(() =>
                sut.Create(request, dummyContext));
            // Assert
            Assert.Contains(request.ToString(), e.Message);
        }
    }
}
