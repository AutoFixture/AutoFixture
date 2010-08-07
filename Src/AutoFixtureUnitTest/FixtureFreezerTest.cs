using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixtureUnitTest
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
        public void FreezeSeededWithNullFixtureThrows()
        {
            // Fixture setup
            var dummySeed = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureFreezer.Freeze<object>(null, dummySeed));
            // Teardown
        }

        [Fact]
        public void FreezeCustomWithNullFixtureThrows()
        {
            // Fixture setup
            Func<ICustomizationComposer<object>, ISpecimenBuilderComposer> dummyTransform = c => c;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureFreezer.Freeze<object>(null, dummyTransform));
            // Teardown
        }
    }
}
