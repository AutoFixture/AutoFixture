using System;
using Xunit;

namespace AutoFixture.SeedExtensions.UnitTest
{
    public class FreezeSeedExtensionsTest
    {
        [Fact]
        public void FreezeSeededWithNullFixtureThrows()
        {
            // Arrange
            var dummySeed = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                FreezeSeedExtensions.Freeze<object>(null, dummySeed));
        }
    }
}
