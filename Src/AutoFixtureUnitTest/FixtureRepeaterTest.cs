using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class FixtureRepeaterTest
    {
        [Fact]
        public void NullIFixtureThrows()
        {
            // Arrange
            IFixture fixture = null;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => fixture.Repeat(() => new object()));
        }

        [Fact]
        public void RepeatWillPerformActionTheDefaultNumberOfTimes()
        {
            // Arrange
            IFixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Act
            int result = 0;
            sut.Repeat(() => result++).ToList();
            // Assert
            Assert.Equal<int>(expectedCount, result);
        }

        [Fact]
        public void RepeatWillReturnTheDefaultNumberOfItems()
        {
            // Arrange
            IFixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Act
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Assert
            Assert.Equal<int>(expectedCount, result.Count());
        }

        [Fact]
        public void RepeatWillPerformActionTheSpecifiedNumberOfTimes()
        {
            // Arrange
            int expectedCount = 2;
            IFixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Act
            int result = 0;
            sut.Repeat(() => result++).ToList();
            // Assert
            Assert.Equal<int>(expectedCount, result);
        }

        [Fact]
        public void RepeatWillReturnTheSpecifiedNumberOfItems()
        {
            // Arrange
            int expectedCount = 13;
            IFixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Act
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Assert
            Assert.Equal<int>(expectedCount, result.Count());
        }
    }
}