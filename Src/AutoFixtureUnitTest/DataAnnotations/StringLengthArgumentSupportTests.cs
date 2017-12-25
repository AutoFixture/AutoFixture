using System.ComponentModel.DataAnnotations;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class StringLengthArgumentSupportTests
    {
        [Fact]
        public void FixtureCorrectlyCreatesShortText()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var actual = fixture.Create<ClassWithShortStringLengthConstrainedConstructorArgument>();
            // Assert
            Assert.True(
                actual.ShortText.Length <= ClassWithShortStringLengthConstrainedConstructorArgument.ShortTextMaximumLength,
                "AutoFixture should respect [StringLength] attribute on constructor arguments.");
        }

        [Fact]
        public void FixtureCorrectlyCreatesLongText()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var actual = fixture.Create<ClassWithLongStringLengthConstrainedConstructorArgument>();
            // Assert
            Assert.Equal(
                ClassWithLongStringLengthConstrainedConstructorArgument.LongTextLength,
                actual.LongText.Length);
        }

        private class ClassWithShortStringLengthConstrainedConstructorArgument
        {
            public const int ShortTextMaximumLength = 3;
            public readonly string ShortText;

            public ClassWithShortStringLengthConstrainedConstructorArgument(
                [StringLength(ShortTextMaximumLength)]string shortText)
            {
                this.ShortText = shortText;
            }
        }

        private class ClassWithLongStringLengthConstrainedConstructorArgument
        {
            public const int LongTextLength = 50;
            public readonly string LongText;

            public ClassWithLongStringLengthConstrainedConstructorArgument(
                [StringLength(LongTextLength, MinimumLength = LongTextLength)]string longText)
            {
                this.LongText = longText;
            }
        }
    }
}
