using System;
using Xunit;

namespace AutoFixtureDocumentationTest.Extension.Constraints
{
    public class ConstrainedStringGeneratorTest
    {
        public ConstrainedStringGeneratorTest()
        {
        }

        [Fact]
        public void CreateWithMaxLessThanZeroWillThrow()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringGenerator(-187, -1));
        }

        [Fact]
        public void CreateWithMinLargerThanMaxWillThrow()
        {
            // Arrange
            // Act
            // Assert (expected exception)
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringGenerator(9, 8));
        }

        [Fact]
        public void CreateAnonymousWillPrefixWithSeed()
        {
            // Arrange
            var seed = "Anonymous seed";
            var sut = new ConstrainedStringGenerator(1, 100);
            // Act
            var result = sut.CreateaAnonymous(seed);
            // Assert
            Assert.StartsWith(seed, result);
        }

        [Fact]
        public void CreateAnonymousWillSatisfyMinimumWhenLow()
        {
            // Arrange
            var minimum = 4;
            var sut = new ConstrainedStringGenerator(minimum, 100);
            // Act
            string result = sut.CreateaAnonymous(string.Empty);
            // Assert
            Assert.True(result.Length >= minimum, "Length greater than minimum");
        }

        [Fact]
        public void CreateAnonymousWillSatisfyMinimumWhenHigh()
        {
            // Arrange
            var minimum = 109;
            var sut = new ConstrainedStringGenerator(minimum, 200);
            // Act
            string result = sut.CreateaAnonymous(string.Empty);
            // Assert
            Assert.True(result.Length >= minimum, "Length greater than minimum");
        }

        [Fact]
        public void CreateAnonymousWillSatisfyMaximumWhenLow()
        {
            // Arrange
            var maximum = 10;
            var sut = new ConstrainedStringGenerator(1, maximum);
            // Act
            var result = sut.CreateaAnonymous(string.Empty);
            // Assert
            Assert.True(result.Length <= maximum, "Length less than maximum");
        }

        [Fact]
        public void CreateAnonymousWillSatisfyMaximumWhenHigh()
        {
            // Arrange
            var maximum = 89;
            var sut = new ConstrainedStringGenerator(1, maximum);
            // Act
            var result = sut.CreateaAnonymous(string.Empty);
            // Assert
            Assert.True(result.Length <= maximum, "Length less than maximum");
        }
    }
}
