using System;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
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
            // Arrange
            var dummyItem = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Inject(null, dummyItem));
        }

        [Fact]
        public void RegisterParameterlessFuncWithNullFixtureThrows()
        {
            // Arrange
            Func<object> dummyFunc = () => new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Register(null, dummyFunc));
        }

        [Fact]
        public void RegisterSingleParameterFuncWithNullFixtureThrows()
        {
            // Arrange
            Func<object, object> dummyFunc = x => x;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Register(null, dummyFunc));
        }

        [Fact]
        public void RegisterDoubleParameterFuncWithNullFixtureThrows()
        {
            // Arrange
            Func<object, object, object> dummyFunc = (x, y) => x;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Register(null, dummyFunc));
        }

        [Fact]
        public void RegisterTripleParameterFuncWithNullFixtureThrows()
        {
            // Arrange
            Func<object, object, object, object> dummyFunc = (x, y, z) => x;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Register(null, dummyFunc));
        }

        [Fact]
        public void RegisterQuadrupleParameterFuncWithNullFixtureThrows()
        {
            // Arrange
            Func<object, object, object, object, object> dummyFunc = (x, y, z, æ) => x;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                FixtureRegistrar.Register(null, dummyFunc));
        }
    }
}
