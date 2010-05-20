using System;
using Ploeh.AutoFixture.Dsl;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class StrategyComposerTest
    {
        [Fact]
        public void SutIsRuleComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = StrategyComposerTest.CreateSut<object>();
            // Verify outcome
            Assert.IsAssignableFrom<IStrategyComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsInstance()
        {
            // Fixture setup
            var sut = StrategyComposerTest.CreateSut<object>();
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void FromNullFactoryThrows()
        {
            // Fixture setup
            var sut = StrategyComposerTest.CreateSut<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.FromSeed(null));
            // Teardown
        }

        [Fact]
        public void FromSeedReturnNewInstance()
        {
            // Fixture setup
            var sut = StrategyComposerTest.CreateSut<object>();
            // Exercise system
            var result = sut.FromSeed(obj => obj);
            // Verify outcome
            Assert.NotSame(sut, result);
            // Teardown
        }

        private static StrategyComposer<T> CreateSut<T>()
        {
            return new StrategyComposer<T>();
        }
    }
}
