using System;
using AutoFixture;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RestrictedEnumTest
    {
        [Fact]
        public void InitializeWithEmptyEnumTypeThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentException>(() => RestrictedEnum.IncludeValues((EmptyEnum)0));
        }

        [Fact]
        public void InitializeWithNoAvailableValuesThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentException>(() => RestrictedEnum.IncludeValues<TriState>());
        }

        [Fact]
        public void InitializeWithNullValuesThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => RestrictedEnum.IncludeValues<TriState>(null));
        }
    }
}
