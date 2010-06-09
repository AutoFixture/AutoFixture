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
        public void InitializeWithDefaultConstructorHasCorrectFactory()
        {
            // Fixture setup
            var sut = new Composer<TypeCode>();
            // Exercise system
            ISpecimenBuilder result = sut.Factory;
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
        public void InitializeWithRootBuilderHasCorrectFactory()
        {
            // Fixture setup
            var expectedFactory = new DelegatingSpecimenBuilder();
            var sut = new Composer<byte>(expectedFactory);
            // Exercise system
            var result = sut.Factory;
            // Verify outcome
            Assert.Equal(expectedFactory, result);
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
            var expectedResult = sut.Factory;
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            var filter = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(result);
            Assert.Equal(expectedResult, filter.Builder);
            var spec = Assert.IsAssignableFrom<ExactTypeSpecification>(filter.Specification);
            Assert.Equal(typeof(int), spec.TargetType);
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
        public void FromSeedReturnsCorrectResult()
        {
            // Fixture setup
            Func<OperatingSystem, OperatingSystem> expectedFactory = s => s;
            var sut = ComposerTest.CreateSut<OperatingSystem>();
            // Exercise system
            var result = sut.FromSeed(expectedFactory);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<OperatingSystem>>(result);
            var factory = Assert.IsAssignableFrom<SeededFactory<OperatingSystem>>(resultingComposer.Factory);
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
        public void FromZeroInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<Uri> expectedFactory = () => new Uri("urn:anonymous:uri");
            var sut = ComposerTest.CreateSut<Uri>();
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<Uri>>(result);
            var factory = Assert.IsAssignableFrom<SpecimenFactory<Uri>>(resultingComposer.Factory);
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
        public void FromSingleInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<Version, Uri> expectedFactory = v => new Uri("urn:" + v.ToString());
            var sut = ComposerTest.CreateSut<Uri>();
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<Uri>>(result);
            var factory = Assert.IsAssignableFrom<SpecimenFactory<Version, Uri>>(resultingComposer.Factory);
            Assert.Equal(expectedFactory, factory.Factory);
            // Teardown
        }

        [Fact]
        public void FromNullDoubleParameterFactoryThrows()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<UnicodeEncoding>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.FromFactory<WeakReference, HttpStyleUriParser>(null));
            // Teardown
        }

        [Fact]
        public void ComposeFromDoubleInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<string, decimal, byte> expectedFactory = (x, y) => 0;
            var sut = ComposerTest.CreateSut<byte>();
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<byte>>(result);
            var factory = Assert.IsAssignableFrom<SpecimenFactory<string, decimal, byte>>(resultingComposer.Factory);
            Assert.Equal(expectedFactory, factory.Factory);
            // Teardown
        }

        [Fact]
        public void FromNullTripleParameterFactoryThrows()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<FileStyleUriParser>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.FromFactory<GenericUriParser, Base64FormattingOptions, LdapStyleUriParser>(null));
            // Teardown
        }

        [Fact]
        public void ComposeFromTripleInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<HttpStyleUriParser, Random, ASCIIEncoding, string> expectedFactory = (x, y, z)=> string.Empty;
            var sut = ComposerTest.CreateSut<string>();
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<string>>(result);
            var factory = Assert.IsAssignableFrom<SpecimenFactory<HttpStyleUriParser, Random, ASCIIEncoding, string>>(resultingComposer.Factory);
            Assert.Equal(expectedFactory, factory.Factory);
            // Teardown
        }

        private static Composer<T> CreateSut<T>()
        {
            return new Composer<T>();
        }
    }
}
