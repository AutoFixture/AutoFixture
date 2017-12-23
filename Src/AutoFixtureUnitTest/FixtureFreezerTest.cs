using System;
using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    /// <summary>
    /// These tests mostly deal with boundary cases (like null
    /// guards) that are specific to the extension methods.
    /// Implementation are covered elsewhere (most notable in
    /// FixtureTest).
    /// </summary>
    public class FixtureFreezerTest
    {
        [Fact]
        public void FreezeUnseededWithNullFixtureThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                FixtureFreezer.Freeze<object>(null));
        }

        [Fact]
        public void FreezeCustomWithNullFixtureThrows()
        {
            // Arrange
            Func<ICustomizationComposer<object>, ISpecimenBuilder> dummyTransform = c => c;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                FixtureFreezer.Freeze<object>(null, dummyTransform));
        }
    }
}
