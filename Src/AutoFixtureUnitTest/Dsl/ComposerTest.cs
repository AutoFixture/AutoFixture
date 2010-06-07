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
            var filter = ComposerTest.AssertComposedBuilder<int>(result);
            Assert.IsAssignableFrom<ModestConstructorInvoker>(filter.Builder);
            // Teardown
        }

        [Fact]
        public void FromNullSeedThrows()
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
            var filter = ComposerTest.AssertComposedBuilder<OperatingSystem>(result);
            var factory = Assert.IsAssignableFrom<SeededFactory<OperatingSystem>>(filter.Builder);
            Assert.Equal(expectedFactory, factory.Factory);
            // Teardown
        }

        [Fact]
        public void FromNullParameterlessFactoryThrows()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<Guid>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.FromFactory((Func<Guid>)null));
            // Teardown
        }

        [Fact]
        public void ComposeFromZeroInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<Uri> expectedFactory = () => new Uri("urn:anonymous:uri");
            var sut = ComposerTest.CreateSut<Uri>();
            // Exercise system
            var result = sut.FromFactory(expectedFactory).Compose();
            // Verify outcome
            var filter = ComposerTest.AssertComposedBuilder<Uri>(result);
            var factory = Assert.IsAssignableFrom<SpecimenFactory<Uri>>(filter.Builder);
            Assert.Equal(expectedFactory, factory.Factory);
            // Teardown
        }

        [Fact]
        public void FromNullSingleParameterFactoryThrows()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<DateTimeKind>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.FromFactory<ObsoleteAttribute>(null));
            // Teardown
        }

        [Fact]
        public void ComposeFromSingleInputFacttoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<Version, Uri> expectedFactory = v => new Uri("urn:" + v.ToString());
            var sut = ComposerTest.CreateSut<Uri>();
            // Exercise system
            var result = sut.FromFactory(expectedFactory).Compose();
            // Verify outcome
            var filter = ComposerTest.AssertComposedBuilder<Uri>(result);
            var factory = Assert.IsAssignableFrom<SpecimenFactory<Version, Uri>>(filter.Builder);
            Assert.Equal(expectedFactory, factory.Factory);
            // Teardown
        }

        private static Composer<T> CreateSut<T>()
        {
            return new Composer<T>();
        }

        private static FilteringSpecimenBuilder AssertComposedBuilder<T>(ISpecimenBuilder builder)
        {
            var filter = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(builder);
            var spec = Assert.IsAssignableFrom<ExactTypeSpecification>(filter.Specification);
            Assert.Equal(typeof(T), spec.TargetType);

            return filter;
        }
    }
}
