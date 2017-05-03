using System;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class NullComposerTest
    {
        [Fact]
        public void SutIsCustomizationComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullComposer<object>();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NullComposer<object>((ISpecimenBuilder)null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NullComposer<object>((Func<ISpecimenBuilder>)null));
            // Teardown
        }

        [Fact]
        public void InitializedWithDefaultConstructorCorrectlyComposes()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
            Assert.Empty(composite.Builders);
            // Teardown
        }

        [Fact]
        public void InitializedWithBuilderCorrectlyComposes()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new NullComposer<object>(expectedBuilder);
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void InitializedWithFuncCorrectlyComposes()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new NullComposer<object>(() => expectedBuilder);
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void FromSeedReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.FromSeed(s => s);
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void FromBuilderFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<Version>();
            // Exercise system
            var dummyFactory = new DelegatingSpecimenBuilder();
            var result = sut.FromFactory(dummyFactory);
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void FromFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.FromFactory(() => new object());
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void FromSingleParameterFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.FromFactory((object x) => new object());
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void FromDoubleParameterFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.FromFactory((object x, object y) => new object());
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void FromTripeParameterFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.FromFactory((object x, object y, object z) => new object());
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void FromQuadrupleParameterFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.FromFactory((object x, object y, object z, object æ) => new object());
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.Do(x => { });
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.OmitAutoProperties();
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void AnonymousWithReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<PropertyHolder<object>>();
            // Exercise system
            var result = sut.With(x => x.Property);
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void WithReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<PropertyHolder<object>>();
            // Exercise system
            var result = sut.With(x => x.Property, new object());
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.WithAutoProperties();
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void WithoutReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<PropertyHolder<object>>();
            // Exercise system
            var result = sut.Without(x => x.Property);
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void MatchReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NullComposer<object>();
            // Exercise system
            var result = sut.Match();
            // Verify outcome
            Assert.IsAssignableFrom<NullMatchComposer<object>>(result);
            // Teardown
        }
    }
}
