using System;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RequireTest
    {
        [Fact]
        public void IsNotNullWithNullShouldThrowArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                Require.IsNotNull(null));
            // Teardown
        }

        [Fact]
        public void IsNotNullWithObjectShouldDoNothing()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Require.IsNotNull(new object());
            // Teardown
        }

        [Fact]
        public void IsNotNullWithNullAndNameShouldThrowArgumentNullExceptionContainingThatName()
        {
            // Fixture setup
            var argumentName = "someArgument";
            // Exercise system and verify outcome
            var exception = Assert.Throws<ArgumentNullException>(() =>
                Require.IsNotNull(null, argumentName));
            Assert.Contains(argumentName, exception.Message);
            // Teardown
        }
    }
}
