using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class StringLengthArgumentSupportTests
    {
        [Fact]
        public void FixtureCorrectlyCreatesShortText()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var actual = fixture.Create<ClassWithLengthConstrainedConstructorArgument>();
            // Verify outcome
            Assert.True(
                actual.ShortText.Length <= ClassWithLengthConstrainedConstructorArgument.ShortTextMaximumLength,
                "AutoFixture should respect [StringLength] attribute on constructor arguments.");
            // Teardown
        }

        [Fact]
        public void FixtureCorrectlyCreatesLongText()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var actual = fixture.Create<ClassWithLengthConstrainedConstructorArgument>();
            // Verify outcome
            Assert.Equal(
                ClassWithLengthConstrainedConstructorArgument.LongTextLength,
                actual.LongText.Length);
            // Teardown
        }

        private class ClassWithLengthConstrainedConstructorArgument
        {
            public const int ShortTextMaximumLength = 3;
            public const int LongTextLength = 50;
            public readonly string ShortText;
            public readonly string LongText;

            public ClassWithLengthConstrainedConstructorArgument(
                [StringLength(ShortTextMaximumLength)]string shortText,
                [StringLength(LongTextLength, MinimumLength = LongTextLength)]string longText)
            {
                this.ShortText = shortText;
                this.LongText = longText;
            }
        }
    }
}
