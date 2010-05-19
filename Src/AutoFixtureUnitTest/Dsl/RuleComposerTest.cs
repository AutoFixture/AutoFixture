using System;
using Ploeh.AutoFixture.Dsl;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class RuleComposerTest
    {
        [Fact]
        public void SutIsRuleComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = RuleComposerTest.CreateSut<object>();
            // Verify outcome
            Assert.IsAssignableFrom<IRuleComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsInstance()
        {
            // Fixture setup
            var sut = RuleComposerTest.CreateSut<object>();
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
            var sut = RuleComposerTest.CreateSut<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.FromSeed(null));
            // Teardown
        }

        [Fact]
        public void FromSeedReturnNewInstance()
        {
            // Fixture setup
            var sut = RuleComposerTest.CreateSut<object>();
            // Exercise system
            var result = sut.FromSeed(obj => obj);
            // Verify outcome
            Assert.NotSame(sut, result);
            // Teardown
        }

        private static RuleComposer<T> CreateSut<T>()
        {
            return new RuleComposer<T>();
        }
    }
}
