using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DisposableTrackingCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new DisposableTrackingCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new DisposableTrackingCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void BehaviorIsInstance()
        {
            // Fixture setup
            var sut = new DisposableTrackingCustomization();
            // Exercise system
            DisposableTrackingBehavior result = sut.Behavior;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void BehaviorIsStable()
        {
            // Fixture setup
            var sut = new DisposableTrackingCustomization();
            var expectedBehavior = sut.Behavior;
            // Exercise system
            var result = sut.Behavior;
            // Verify outcome
            Assert.Equal(expectedBehavior, result);
            // Teardown
        }

        [Fact]
        public void CustomizeAddsCorrectBehavior()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new DisposableTrackingCustomization();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.Contains(sut.Behavior, fixture.Behaviors);
            // Teardown
        }

        [Fact]
        public void SutIsDisposable()
        {
            // Fixture setup
            // Exercise system
            var sut = new DisposableTrackingCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<IDisposable>(sut);
            // Teardown
        }
    }
}
