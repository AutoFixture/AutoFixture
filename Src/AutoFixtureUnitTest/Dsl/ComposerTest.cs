using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;

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
        public void InitializeWithDefaultConstructorHasCorrectPostprocessors()
        {
            // Fixture setup
            var sut = new Composer<HttpStyleUriParser>();
            // Exercise system
            IEnumerable<ISpecifiedSpecimenCommand<HttpStyleUriParser>> result = sut.Postprocessors;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void IntializeWithNullFactoryThrows()
        {
            // Fixture setup
            var dummyCommands = Enumerable.Empty<ISpecifiedSpecimenCommand<UriParser>>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Composer<UriParser>(null, dummyCommands));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullPostprocessorsThrows()
        {
            // Fixture setup
            var dummyFactory = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Composer<ParamArrayAttribute>(dummyFactory, null));
            // Teardown
        }

        [Fact]
        public void InitializeWithFactoryHasCorrectFactory()
        {
            // Fixture setup
            var expectedFactory = new DelegatingSpecimenBuilder();
            var dummyCommands = Enumerable.Empty<ISpecifiedSpecimenCommand<byte>>();
            var sut = new Composer<byte>(expectedFactory, dummyCommands);
            // Exercise system
            var result = sut.Factory;
            // Verify outcome
            Assert.Equal(expectedFactory, result);
            // Teardown
        }

        [Fact]
        public void IntializeWithPostprocessorsHasCorrectCommands()
        {
            // Fixture setup
            var dummyFactory = new DelegatingSpecimenBuilder();
            var expectedCommands = Enumerable.Range(1, 3)
                .Select(i => new DelegatingSpecifiedSpecimenCommand<Version>())
                .Cast<ISpecifiedSpecimenCommand<Version>>()
                .ToList();
            var sut = new Composer<Version>(dummyFactory, expectedCommands);
            // Exercise system
            var result = sut.Postprocessors;
            // Verify outcome
            Assert.True(expectedCommands.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void PostprocessorEnumerationIsStable()
        {
            // Fixture setup
            var dummyFactory = new DelegatingSpecimenBuilder();
            var postprocessors = Enumerable.Range(1, 3)
                .Select(i => new DelegatingSpecifiedSpecimenCommand<Uri>())
                .Cast<ISpecifiedSpecimenCommand<Uri>>();
            var sut = new Composer<Uri>(dummyFactory, postprocessors);
            var expected = sut.Postprocessors;
            // Exercise system
            var result = sut.Postprocessors;
            // Verify outcome
            Assert.True(expected.SequenceEqual(result));
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
        public void WithNullFactoryThrows()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.WithFactory(null));
            // Teardown
        }

        [Fact]
        public void WithFactoryReturnsResultWithCorrectFactory()
        {
            // Fixture setup
            var expectedFactory = new DelegatingSpecimenBuilder();
            var sut = ComposerTest.CreateSut<int>();
            // Exercise system
            var result = sut.WithFactory(expectedFactory);
            // Verify outcome
            Assert.Equal(expectedFactory, result.Factory);
            // Teardown
        }

        [Fact]
        public void WithFactoryReturnsResultWithCorrectPostprocessors()
        {
            // Fixture setup
            var expectedPostprocessors = Enumerable.Range(1, 3)
                .Select(i => new DelegatingSpecifiedSpecimenCommand<Version>())
                .Cast<ISpecifiedSpecimenCommand<Version>>()
                .ToList();
            var sut = ComposerTest.CreateSut<Version>(expectedPostprocessors);
            // Exercise system
            var dummyFactory = new DelegatingSpecimenBuilder();
            var result = sut.WithFactory(dummyFactory);
            // Verify outcome
            Assert.True(expectedPostprocessors.SequenceEqual(result.Postprocessors));
            // Teardown
        }

        [Fact]
        public void WithNullPostprocessorThrows()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.WithPostprocessor(null));
            // Teardown
        }

        [Fact]
        public void WithPostprocessorReturnsResultWithCorrectFactory()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<object>();
            var expectedFactory = sut.Factory;
            // Exercise system
            var dummyPostprocessor = new DelegatingSpecifiedSpecimenCommand<object>();
            var result = sut.WithPostprocessor(dummyPostprocessor);
            // Verify outcome
            Assert.Equal(expectedFactory, result.Factory);
            // Teardown
        }

        [Fact]
        public void WithPostprocessorReturnsResultWithAddedPostprocessor()
        {
            // Fixture setup
            var expectedPostprocessor = new DelegatingSpecifiedSpecimenCommand<object>();
            var sut = ComposerTest.CreateSut<object>();
            // Exercise system
            var result = sut.WithPostprocessor(expectedPostprocessor);
            // Verify outcome
            Assert.Contains(expectedPostprocessor, result.Postprocessors);
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
        public void FromDoubleInputFactoryReturnsCorrectResult()
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
        public void FromTripleInputFactoryReturnsCorrectResult()
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

        [Fact]
        public void FromNullQuadrupleParameterFactoryThrows()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<GenericUriParser>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.FromFactory<int, DateTime, AttributeTargets, LoaderOptimization>(null));
            // Teardown
        }

        [Fact]
        public void FromQuadrupleInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<Random, HttpStyleUriParser, LoaderOptimizationAttribute, Base64FormattingOptions, NetPipeStyleUriParser> expectedFactory = (x, y, z, æ) => new NetPipeStyleUriParser();
            var sut = ComposerTest.CreateSut<NetPipeStyleUriParser>();
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<NetPipeStyleUriParser>>(result);
            var factory = Assert.IsAssignableFrom<SpecimenFactory<Random, HttpStyleUriParser, LoaderOptimizationAttribute, Base64FormattingOptions, NetPipeStyleUriParser>>(resultingComposer.Factory);
            Assert.Equal(expectedFactory, factory.Factory);
            // Teardown
        }

        [Fact]
        public void WithNullPropertyPickerThrows()
        {
            // Fixture setup
            var sut = ComposerTest.CreateSut<UriPartial>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Without<ConsoleCancelEventArgs>(null));
            // Teardown
        }

        [Fact]
        public void WithoutReturnsCorrectResult()
        {
            // Fixture setup
            var expectedMember = typeof(PropertyHolder<string>).GetProperty("Property");
            var sut = ComposerTest.CreateSut<PropertyHolder<string>>();
            // Exercise system
            var result = sut.Without(x => x.Property);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<PropertyHolder<string>>>(result);
            var postprocessor = resultingComposer.Postprocessors.OfType<NullSpecifiedSpecimenCommand<PropertyHolder<string>, string>>().Single();
            Assert.Equal(expectedMember, postprocessor.Member);
            // Teardown
        }

        private static Composer<T> CreateSut<T>()
        {
            return new Composer<T>();
        }

        private static Composer<T> CreateSut<T>(IEnumerable<ISpecifiedSpecimenCommand<T>> postprocessors)
        {
            return new Composer<T>(new ModestConstructorInvoker(), postprocessors);
        }
    }
}
