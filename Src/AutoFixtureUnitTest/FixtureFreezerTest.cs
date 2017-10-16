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
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureFreezer.Freeze<object>(null));
            // Teardown
        }

        [Fact]
        public void FreezeCustomWithNullFixtureThrows()
        {
            // Fixture setup
            Func<ICustomizationComposer<object>, ISpecimenBuilder> dummyTransform = c => c;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureFreezer.Freeze<object>(null, dummyTransform));
            // Teardown
        }
    }
}
