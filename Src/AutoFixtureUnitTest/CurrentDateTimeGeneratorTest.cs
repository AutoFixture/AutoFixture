using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class CurrentDateTimeGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new CurrentDateTimeGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new CurrentDateTimeGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Arrange
            var sut = new CurrentDateTimeGenerator();
            // Act & assert
            var dummyRequest = new object();
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
        }

        [Fact]
        public void CreateWithNonDateTimeRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonDateTimeRequest = new object();
            var sut = new CurrentDateTimeGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonDateTimeRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsCorrectResult()
        {
            // Arrange
            var before = DateTime.Now;
            var dateTimeRequest = typeof(DateTime);
            var sut = new CurrentDateTimeGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dateTimeRequest, dummyContext);
            // Assert
            var after = DateTime.Now;
            var dt = Assert.IsAssignableFrom<DateTime>(result);
            Assert.True(before <= dt && dt <= after);
        }
    }
}