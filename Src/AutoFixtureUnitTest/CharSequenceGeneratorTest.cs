using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class CharSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new CharSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new CharSequenceGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Arrange
            var sut = new CharSequenceGenerator();
            var dummyRequest = new object();
            // Act & assert
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
        }

        [Fact]
        public void CreateWithNonCharRequestReturnsCorrectResult()
        {
            // Arrange
            var dummyRequest = new object();
            var sut = new CharSequenceGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithCharRequestReturnsCorrectResult()
        {
            // Arrange
            var charRequest = typeof(char);
            var sut = new CharSequenceGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(charRequest, dummyContext);
            // Assert
            Assert.Equal('!', result);
        }

        [Fact]
        public void CreateWithCharRequestReturnsCorrectResultOnSecondCall()
        {
            // Arrange
            var charRequest = typeof(char);
            var sut = new CharSequenceGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable.Range(1, 2)
                .Select(i => sut.Create(charRequest, dummyContext))
                .Cast<char>();
            // Assert
            char c = ' ';
            IEnumerable<char> expectedResult = Enumerable.Range(1, 2).Select(i => ++c);
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateWithCharRequestReturnsCorrectResultOnTenthCall()
        {
            // Arrange
            var charRequest = typeof(char);
            var sut = new CharSequenceGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable.Range(1, 10)
                .Select(i => sut.Create(charRequest, dummyContext))
                .Cast<char>();
            // Assert
            char c = '!';
            IEnumerable<char> expectedResult = Enumerable.Range(1, 10).Select(i => c++);
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void CreateWithCharReturnsCorrectResultWhenRunOutOfChars()
        {
            // Arrange
            var charRequest = typeof(char);
            var sut = new CharSequenceGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = Enumerable.Range(1, 95)
                .Select(i => sut.Create(charRequest, dummyContext))
                .Cast<char>()
                .Last();
            // Assert
            char expectedResult = '!';
            Assert.Equal(expectedResult, result);
        }
    }
}
