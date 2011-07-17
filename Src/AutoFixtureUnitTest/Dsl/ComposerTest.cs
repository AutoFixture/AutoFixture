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
            Assert.IsAssignableFrom<MethodInvoker>(result);
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
        public void InitializeWithDefaultConstructorHasCorrectEnableAutoProperties()
        {
            // Fixture setup
            var sut = new Composer<PlatformID>();
            // Exercise system
            bool result = sut.EnableAutoProperties;
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IntializeWithNullFactoryThrows()
        {
            // Fixture setup
            var dummyCommands = Enumerable.Empty<ISpecifiedSpecimenCommand<UriParser>>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Composer<UriParser>(null, dummyCommands, false));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullPostprocessorsThrows()
        {
            // Fixture setup
            var dummyFactory = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Composer<ParamArrayAttribute>(dummyFactory, null, false));
            // Teardown
        }

        [Fact]
        public void InitializeWithFactoryHasCorrectFactory()
        {
            // Fixture setup
            var expectedFactory = new DelegatingSpecimenBuilder();
            var dummyCommands = Enumerable.Empty<ISpecifiedSpecimenCommand<byte>>();
            var sut = new Composer<byte>(expectedFactory, dummyCommands, false);
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
            var sut = new Composer<Version>(dummyFactory, expectedCommands, false);
            // Exercise system
            var result = sut.Postprocessors;
            // Verify outcome
            Assert.True(expectedCommands.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithEnableAutoPropertiesHasMatchingProperty()
        {
            // Fixture setup
            var dummyFactory = new DelegatingSpecimenBuilder();
            var dummyPostprocessors = Enumerable.Empty<ISpecifiedSpecimenCommand<HttpStyleUriParser>>();
            var enableAutoProperties = true;
            var sut = new Composer<HttpStyleUriParser>(dummyFactory, dummyPostprocessors, enableAutoProperties);
            // Exercise system
            var result = sut.EnableAutoProperties;
            // Verify outcome
            Assert.Equal(enableAutoProperties, result);
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
            var sut = new Composer<Uri>(dummyFactory, postprocessors, false);
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
            var sut = new SutBuilder<object>().Create();
            // Verify outcome
            Assert.IsAssignableFrom<IFactoryComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void WithNullFactoryThrows()
        {
            // Fixture setup
            var sut = new SutBuilder<object>().Create();
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
            var sut = new SutBuilder<int>().Create();
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
            var sut = new SutBuilder<Version>().With(expectedPostprocessors).Create();
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
            var sut = new SutBuilder<object>().Create();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.WithPostprocessor(null));
            // Teardown
        }

        [Fact]
        public void WithPostprocessorReturnsResultWithCorrectFactory()
        {
            // Fixture setup
            var sut = new SutBuilder<object>().Create();
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
            var sut = new SutBuilder<object>().Create();
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
            var sut = new SutBuilder<int>().Create();
            var expectedResult = sut.Factory;
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            Func<ISpecimenBuilder, bool> factoryPredicate = b =>
            {
                var guard = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(b);
                return guard.Builder == expectedResult;
            };
            result.IsFilter().ShouldContain(factoryPredicate).ShouldSpecify<int>();
            // Teardown
        }

        [Fact]
        public void ComposeWithPostprocessorsReturnsCorrectResult()
        {
            // Fixture setup
            var postproc1 = new DelegatingSpecifiedSpecimenCommand<string>();
            var postproc2 = new DelegatingSpecifiedSpecimenCommand<string>();
            var postproc3 = new DelegatingSpecifiedSpecimenCommand<string>();

            var sut = new SutBuilder<string>().With(new[] { postproc1, postproc2, postproc3 }).Create();
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            var filter = result.IsFilter().ShouldSpecify<string>();

            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(filter.Builder);

            var pp1 = Assert.IsAssignableFrom<Postprocessor<string>>(composite.Builders.First());
            Assert.Equal(postproc3.Execute, pp1.Action);

            var pp2 = Assert.IsAssignableFrom<Postprocessor<string>>(pp1.Builder);
            Assert.Equal(postproc2.Execute, pp2.Action);

            var pp3 = Assert.IsAssignableFrom<Postprocessor<string>>(pp2.Builder);
            Assert.Equal(postproc1.Execute, pp3.Action);

            var guard = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(pp3.Builder);

            Assert.Equal(sut.Factory, guard.Builder);
            // Teardown
        }

        [Fact]
        public void FromNullSeedThrows()
        {
            // Fixture setup
            var sut = new SutBuilder<object>().Create();
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
            var sut = new SutBuilder<OperatingSystem>().Create();
            // Exercise system
            var result = sut.FromSeed(expectedFactory);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<OperatingSystem>>(result);
            var factory = Assert.IsAssignableFrom<SeededFactory<OperatingSystem>>(resultingComposer.Factory);
            Assert.Equal(expectedFactory, factory.Factory);
            // Teardown
        }

        [Fact]
        public void FromBuilderFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var expectedFactory = new DelegatingSpecimenBuilder();
            var sut = new SutBuilder<Version>().Create();
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<Version>>(result);
            Assert.Equal(expectedFactory, resultingComposer.Factory);
            // Teardown
        }

        [Fact]
        public void FromNullParameterlessFactoryThrows()
        {
            // Fixture setup
            var sut = new SutBuilder<Guid>().Create();
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
            var sut = new SutBuilder<Uri>().Create();
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
            var sut = new SutBuilder<DateTimeKind>().Create();
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
            var sut = new SutBuilder<Uri>().Create();
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
            var sut = new SutBuilder<UnicodeEncoding>().Create();
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
            var sut = new SutBuilder<byte>().Create(); ;
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
            var sut = new SutBuilder<FileStyleUriParser>().Create();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.FromFactory<GenericUriParser, Base64FormattingOptions, LdapStyleUriParser>(null));
            // Teardown
        }

        [Fact]
        public void FromTripleInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<HttpStyleUriParser, Random, ASCIIEncoding, string> expectedFactory = (x, y, z) => string.Empty;
            var sut = new SutBuilder<string>().Create();
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
            var sut = new SutBuilder<GenericUriParser>().Create();
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
            var sut = new SutBuilder<NetPipeStyleUriParser>().Create();
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<NetPipeStyleUriParser>>(result);
            var factory = Assert.IsAssignableFrom<SpecimenFactory<Random, HttpStyleUriParser, LoaderOptimizationAttribute, Base64FormattingOptions, NetPipeStyleUriParser>>(resultingComposer.Factory);
            Assert.Equal(expectedFactory, factory.Factory);
            // Teardown
        }

        [Fact]
        public void WithoutNullPropertyPickerThrows()
        {
            // Fixture setup
            var sut = new SutBuilder<UriPartial>().Create();
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
            var sut = new SutBuilder<PropertyHolder<string>>().Create();
            // Exercise system
            var result = sut.Without(x => x.Property);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<PropertyHolder<string>>>(result);
            var postprocessor = resultingComposer.Postprocessors.OfType<SpecifiedNullCommand<PropertyHolder<string>, string>>().Single();
            Assert.Equal(expectedMember, postprocessor.Member);
            // Teardown
        }

        [Fact]
        public void WithNullPropertyPickerAndDummyValueThrows()
        {
            // Fixture setup
            var sut = new SutBuilder<object>().Create();
            var dummyValue = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.With<object>(null, dummyValue));
            // Teardown
        }

        [Fact]
        public void WithNullValueDoesNotThrow()
        {
            // Fixture setup
            var sut = new SutBuilder<PropertyHolder<object>>().Create();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.With(ph => ph.Property, null));
            // Teardown
        }

        [Fact]
        public void WithValueReturnsCorrectResult()
        {
            // Fixture setup
            var expectedMember = typeof(PropertyHolder<decimal>).GetProperty("Property");
            var expectedValue = 1.3m;
            var sut = new SutBuilder<PropertyHolder<decimal>>().Create();
            // Exercise system
            var result = sut.With(x => x.Property, expectedValue);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<PropertyHolder<decimal>>>(result);
            var postprocessor = resultingComposer.Postprocessors.OfType<BindingCommand<PropertyHolder<decimal>, decimal>>().Single();
            Assert.Equal(expectedMember, postprocessor.Member);
            Assert.Equal(expectedValue, postprocessor.ValueCreator(new DelegatingSpecimenContext()));
            // Teardown
        }

        [Fact]
        public void WithNullPropertyPickerThrows()
        {
            // Fixture setup
            var sut = new SutBuilder<object>().Create();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.With<object>(null));
            // Teardown
        }

        [Fact]
        public void WithReturnsCorrectResult()
        {
            // Fixture setup
            var member = typeof(PropertyHolder<float>).GetProperty("Property");
            var sut = new SutBuilder<PropertyHolder<float>>().Create();
            // Exercise system
            var result = sut.With(x => x.Property);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<PropertyHolder<float>>>(result);
            var postprocessor = resultingComposer.Postprocessors.OfType<BindingCommand<PropertyHolder<float>, float>>().Single();
            Assert.Equal(member, postprocessor.Member);

            object expectedValue = 3.6f;
            var actual = postprocessor.ValueCreator(new DelegatingSpecimenContext { OnResolve = r => r.Equals(member) ? expectedValue : new NoSpecimen(r) });
            Assert.Equal(expectedValue, actual);
            // Teardown
        }

        [Fact]
        public void DoNullActionThrows()
        {
            // Fixture setup
            var sut = new SutBuilder<object>().Create();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Do(null));
            // Teardown
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Fixture setup
            Action<long> expectedAction = s => { };
            var sut = new SutBuilder<long>().Create();
            // Exercise system
            var result = sut.Do(expectedAction);
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<long>>(result);
            var postprocessor = resultingComposer.Postprocessors.OfType<UnspecifiedSpecimenCommand<long>>().Single();
            Assert.Equal(expectedAction, postprocessor.Action);
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new SutBuilder<object>().WithAutoProperties(true).Create();
            // Exercise system
            var result = sut.OmitAutoProperties();
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<object>>(result);
            Assert.Equal(sut.Factory, resultingComposer.Factory);
            Assert.True(sut.Postprocessors.SequenceEqual(resultingComposer.Postprocessors));
            Assert.False(resultingComposer.EnableAutoProperties);
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new SutBuilder<object>().WithAutoProperties(false).Create();
            // Exercise system
            var result = sut.WithAutoProperties();
            // Verify outcome
            var resultingComposer = Assert.IsAssignableFrom<Composer<object>>(result);
            Assert.Equal(sut.Factory, resultingComposer.Factory);
            Assert.True(sut.Postprocessors.SequenceEqual(resultingComposer.Postprocessors));
            Assert.True(resultingComposer.EnableAutoProperties);
            // Teardown
        }

        [Fact]
        public void WithExplicitAutoPropertiesReturnsResultWithCorrectAutoProperties()
        {
            // Fixture setup
            var sut = new SutBuilder<object>().WithAutoProperties(true).Create();
            // Exercise system
            var result = sut.WithAutoProperties(true);
            // Verify outcome
            Assert.True(result.EnableAutoProperties);
            // Teardown
        }

        [Fact]
        public void WithExplicitAutoPropertiesReturnsResultWithCorrectFactory()
        {
            // Fixture setup
            var sut = new SutBuilder<object>().Create();
            // Exercise system
            var result = sut.WithAutoProperties(true);
            // Verify outcome
            Assert.Equal(sut.Factory, result.Factory);
            // Teardown
        }

        [Fact]
        public void WithExplicitAutoPropertiesReturnsResultWithCorrectPostprocessors()
        {
            // Fixture setup
            var postprocessors = Enumerable.Range(1, 3).Select(i => new DelegatingSpecifiedSpecimenCommand<int>()).ToArray();
            var sut = new SutBuilder<int>().With(postprocessors).Create();
            // Exercise system
            var result = sut.WithAutoProperties(false);
            // Verify outcome
            Assert.True(postprocessors.SequenceEqual(result.Postprocessors));
            // Teardown
        }

        [Fact]
        public void ComposeWithoutAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new SutBuilder<string>().WithAutoProperties(false).Create();
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            Func<ISpecimenBuilder, bool> factoryPredicate = b =>
            {
                var guard = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(b);
                return guard.Builder == sut.Factory;
            };
            result.IsFilter().ShouldContain(factoryPredicate).ShouldSpecify<string>();
            // Teardown
        }

        [Fact]
        public void ComposeWithAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new SutBuilder<decimal>().WithAutoProperties(true).Create();
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            var filter = result.IsFilter().ShouldSpecify<decimal>();
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(filter.Builder);
            var postprocessor = Assert.IsAssignableFrom<Postprocessor<decimal>>(composite.Builders.First());
            var guard = Assert.IsAssignableFrom<NoSpecimenOutputGuard>(postprocessor.Builder);
            Assert.Equal(sut.Factory, guard.Builder);
            // Teardown
        }

        private class SutBuilder<T>
        {
            private ISpecimenBuilder factory;
            private List<ISpecifiedSpecimenCommand<T>> postprocessors;
            private bool enableAutoProperties;

            internal SutBuilder()
            {
                this.factory = new MethodInvoker(new ModestConstructorQuery());
                this.postprocessors = new List<ISpecifiedSpecimenCommand<T>>();
            }

            internal SutBuilder<T> With(IEnumerable<ISpecifiedSpecimenCommand<T>> postprocessors)
            {
                this.postprocessors = postprocessors.ToList();
                return this;
            }

            internal SutBuilder<T> WithAutoProperties(bool enable)
            {
                this.enableAutoProperties = enable;
                return this;
            }

            internal Composer<T> Create()
            {
                return new Composer<T>(this.factory, this.postprocessors, this.enableAutoProperties);
            }
        }
    }
}
