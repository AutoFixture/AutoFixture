using System;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    /// <summary>
    /// These tests mostly deal with boundary cases (like null
    /// guards) that are specific to the extension methods.
    /// Implementation are covered elsewhere (most notable in
    /// FixtureTest).
    /// </summary>
    public class FixtureRegistrarTest
    {
        [Fact]
        public void InjectIntoNullFixtureThrows()
        {
            // Fixture setup
            var dummyItem = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Inject(null, dummyItem));
            // Teardown
        }

        [Fact]
        public void RegisterParameterlessFuncWithNullFixtureThrows()
        {
            // Fixture setup
            Func<object> dummyFunc = () => new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Register(null, dummyFunc));
            // Teardown
        }

        [Fact]
        public void RegisterSingleParameterFuncWithNullFixtureThrows()
        {
            // Fixture setup
            Func<object, object> dummyFunc = x => x;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Register(null, dummyFunc));
            // Teardown
        }

        [Fact]
        public void RegisterDoubleParameterFuncWithNullFixtureThrows()
        {
            // Fixture setup
            Func<object, object, object> dummyFunc = (x, y) => x;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Register(null, dummyFunc));
            // Teardown
        }

        [Fact]
        public void RegisterTripleParameterFuncWithNullFixtureThrows()
        {
            // Fixture setup
            Func<object, object, object, object> dummyFunc = (x, y, z) => x;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Register(null, dummyFunc));
            // Teardown
        }

        [Fact]
        public void RegisterQuadrupleParameterFuncWithNullFixtureThrows()
        {
            // Fixture setup
            Func<object, object, object, object, object> dummyFunc = (x, y, z, æ) => x;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Register(null, dummyFunc));
            // Teardown
        }
    }
}
