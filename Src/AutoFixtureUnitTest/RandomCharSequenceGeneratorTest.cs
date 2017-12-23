using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RandomCharSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new RandomCharSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithDefaultConstructorDoesNotThrow()
        {
            // Arrange
            // Act & assert
            Assert.Null(
                Record.Exception(() => new RandomCharSequenceGenerator()));
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Act
            var result = sut.Create(null, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Arrange
            var dummyRequest = new object();
            var sut = new RandomCharSequenceGenerator();
            // Act & assert
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(int))]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Act
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(long))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(string))]
        [InlineData(typeof(object))]
        [InlineData(typeof(decimal))]
        public void CreateWithNonCharTypeRequestReturnsNoSpecimen(Type request)
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Act
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithCharRequestReturnsCorrectResult()
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Act
            var result = sut.Create(typeof(char), dummyContext);
            // Assert
            Assert.IsAssignableFrom<char>(result);
        }

        [Fact]
        public void CreateReturnsCorrectResultOnMultipleCall()
        {
            // Arrange
            int printableCharactersCount = 94;
            char c = '!';
            var expected = Enumerable
                .Range(1, printableCharactersCount)
                .Select(x => c++)
                .Cast<char>()
                .OrderBy(x => x);
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Act
            var result = Enumerable
                .Range(1, printableCharactersCount)
                .Select(x => sut.Create(typeof(char), dummyContext))
                .Cast<char>()
                .OrderBy(x => x);
            // Assert
            Assert.True(expected.SequenceEqual(result));
        }

        [Theory]
        [InlineData(94, 94)]
        [InlineData(94, 100)]
        [InlineData(94, 1000)]
        public void CreateReturnsCorrectResultOnMultipleCallWhenRunOutOfChars(
            int expected, int repeatCount)
        {
            // Arrange
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomCharSequenceGenerator();
            // Act
            var result = Enumerable
                .Range(1, repeatCount)
                .Select(x => sut.Create(typeof(char), dummyContext))
                .Cast<char>()
                .Distinct()
                .Count();
            // Assert
            Assert.Equal(expected, result);
        }
    }
}
