using System;
using Xunit;

namespace AutoFixture.SeedExtensions.UnitTest
{
    public class FreezeSeedExtensionsTest
    {
        [Fact]
        public void FreezeSeededWithNullFixtureThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FreezeSeedExtensions.Freeze<object>(null, dummySeed));
            // Teardown
        }
    }
}
