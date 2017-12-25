using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class BooleanSwitchTest
    {
        [Fact]
        [Obsolete]
        public void CreateWillReturnTrueOnFirstCall()
        {
            // Arrange
            BooleanSwitch sut = new BooleanSwitch();
            // Act
            bool result = sut.Create();
            // Assert
            Assert.True(result, "CreateAnonymous called an uneven number of times");
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTrueOnFirstCall()
        {
            // Arrange
            BooleanSwitch sut = new BooleanSwitch();
            // Act
            bool result = sut.CreateAnonymous();
            // Assert
            Assert.True(result, "CreateAnonymous called an uneven number of times");
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnFalseOnSecondCall()
        {
            // Arrange
            BooleanSwitch sut = new BooleanSwitch();
            sut.Create();
            // Act
            bool result = sut.Create();
            // Assert
            Assert.False(result, "CreateAnonymous called an even number of times");
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnFalseOnSecondCall()
        {
            // Arrange
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            // Act
            bool result = sut.CreateAnonymous();
            // Assert
            Assert.False(result, "CreateAnonymous called an even number of times");
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTrueOnThirdCall()
        {
            // Arrange
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            // Act
            bool result = sut.CreateAnonymous();
            // Assert
            Assert.True(result, "CreateAnonymous called an uneven number of times");
        }
        [Fact]
        [Obsolete]
        public void CreateWillReturnTrueOnThirdCall()
        {
            // Arrange
            BooleanSwitch sut = new BooleanSwitch();
            sut.Create();
            sut.Create();
            // Act
            bool result = sut.Create();
            // Assert
            Assert.True(result, "CreateAnonymous called an uneven number of times");
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnFalseOnFourthCall()
        {
            // Arrange
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            // Act
            bool result = sut.CreateAnonymous();
            // Assert
            Assert.False(result, "CreateAnonymous called an even number of times");
        }
        [Fact]
        [Obsolete]
        public void CreateWillReturnFalseOnFourthCall()
        {
            // Arrange
            BooleanSwitch sut = new BooleanSwitch();
            sut.Create();
            sut.Create();
            sut.Create();
            // Act
            bool result = sut.Create();
            // Assert
            Assert.False(result, "CreateAnonymous called an even number of times");
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new BooleanSwitch();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new BooleanSwitch();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Arrange
            var sut = new BooleanSwitch();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonBooleanRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonBooleanRequest = new object();
            var sut = new BooleanSwitch();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonBooleanRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnFirstCall()
        {
            // Arrange
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(booleanRequest, dummyContainer);
            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(booleanRequest, dummyContainer);
            var result = sut.Create(booleanRequest, dummyContainer);
            // Assert
            Assert.False((bool)result);
        }

        [Fact]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnThirdCall()
        {
            // Arrange
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(booleanRequest, dummyContainer);
            sut.Create(booleanRequest, dummyContainer);
            var result = sut.Create(booleanRequest, dummyContainer);
            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnFourthCall()
        {
            // Arrange
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(booleanRequest, dummyContainer);
            sut.Create(booleanRequest, dummyContainer);
            sut.Create(booleanRequest, dummyContainer);
            var result = sut.Create(booleanRequest, dummyContainer);
            // Assert
            Assert.False((bool)result);
        }
    }
}
