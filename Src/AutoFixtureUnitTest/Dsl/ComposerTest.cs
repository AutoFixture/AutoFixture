using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class ComposerTest
    {
        [Fact]
        public void InitializeWithDefaultConstructorHasCorrectRootBuilder()
        {
            // Fixture setup
            var sut = new Composer<TypeCode>();
            // Exercise system
            ISpecimenBuilder result = sut.RootBuilder;
            // Verify outcome
            Assert.IsAssignableFrom<ModestConstructorInvoker>(result);
            // Teardown
        }

        [Fact]
        public void IntializeWithNullRootBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Composer<UriParser>(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithRootBuilderHasCorrectRootBuilder()
        {
            // Fixture setup
            var expectedRoot = new DelegatingSpecimenBuilder();
            var sut = new Composer<byte>(expectedRoot);
            // Exercise system
            var result = sut.RootBuilder;
            // Verify outcome
            Assert.Equal(expectedRoot, result);
            // Teardown
        }

        [Fact]
        public void SutIsFactoryComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = ComposerTest.CreateSut<object>();
            // Verify outcome
            Assert.IsAssignableFrom<IFactoryComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<int>();
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            var filter = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(result);
            var spec = Assert.IsAssignableFrom<ExactTypeSpecification>(filter.Specification);
            Assert.Equal(typeof(int), spec.TargetType);
            Assert.IsAssignableFrom<ModestConstructorInvoker>(filter.Builder);
            // Teardown
        }

        [Fact]
        public void FromNullFactoryThrows()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.FromSeed(null));
            // Teardown
        }

        [Fact]
        public void ComposeFromSeedReturnsCorrectResult()
        {
            // Fixture setup
            Func<OperatingSystem, OperatingSystem> expectedFactory = s => s;
            var sut = ComposerTest.CreateSut<OperatingSystem>();
            // Exercise system
            var result = sut.FromSeed(expectedFactory).Compose();
            // Verify outcome
            var filter = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(result);
            var spec = Assert.IsAssignableFrom<ExactTypeSpecification>(filter.Specification);
            Assert.Equal(typeof(OperatingSystem), spec.TargetType);
            var factory = Assert.IsAssignableFrom<SeededFactory<OperatingSystem>>(filter.Builder);
            Assert.Equal(expectedFactory, factory.Factory);
            // Teardown
        }

        private static Composer<T> CreateSut<T>()
        {
            return new Composer<T>();
        }
    }
}
