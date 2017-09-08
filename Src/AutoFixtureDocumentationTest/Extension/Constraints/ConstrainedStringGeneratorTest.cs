using System;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Extension.Constraints
{
    public class ConstrainedStringGeneratorTest
    {
        public ConstrainedStringGeneratorTest()
        {
        }

        [Fact]
        public void CreateWithMaxLessThanZeroWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringGenerator(-187, -1));
            // Teardown
        }

        [Fact]
        public void CreateWithMinLargerThanMaxWillThrow()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome (expected exception)
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ConstrainedStringGenerator(9, 8));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillPrefixWithSeed()
        {
            // Fixture setup
            var seed = "Anonymous seed";
            var sut = new ConstrainedStringGenerator(1, 100);
            // Exercise system
            var result = sut.CreateaAnonymous(seed);
            // Verify outcome
            Assert.StartsWith(seed, result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillSatisfyMinimumWhenLow()
        {
            // Fixture setup
            var minimum = 4;
            var sut = new ConstrainedStringGenerator(minimum, 100);
            // Exercise system
            string result = sut.CreateaAnonymous(string.Empty);
            // Verify outcome
            Assert.True(result.Length >= minimum, "Length greater than minimum");
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillSatisfyMinimumWhenHigh()
        {
            // Fixture setup
            var minimum = 109;
            var sut = new ConstrainedStringGenerator(minimum, 200);
            // Exercise system
            string result = sut.CreateaAnonymous(string.Empty);
            // Verify outcome
            Assert.True(result.Length >= minimum, "Length greater than minimum");
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillSatisfyMaximumWhenLow()
        {
            // Fixture setup
            var maximum = 10;
            var sut = new ConstrainedStringGenerator(1, maximum);
            // Exercise system
            var result = sut.CreateaAnonymous(string.Empty);
            // Verify outcome
            Assert.True(result.Length <= maximum, "Length less than maximum");
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillSatisfyMaximumWhenHigh()
        {
            // Fixture setup
            var maximum = 89;
            var sut = new ConstrainedStringGenerator(1, maximum);
            // Exercise system
            var result = sut.CreateaAnonymous(string.Empty);
            // Verify outcome
            Assert.True(result.Length <= maximum, "Length less than maximum");
            // Teardown
        }
    }
}
