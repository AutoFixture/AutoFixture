using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class MultipleCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new MultipleCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new MultipleCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(EnumerableRelay))]
        [InlineData(typeof(ListRelay))]
        [InlineData(typeof(CollectionRelay))]
        [InlineData(typeof(DictionaryRelay))]
        public void CustomizeAddsRelayToFixture(Type relayType)
        {
            // Fixture setup
            var sut = new MultipleCustomization();
            var fixture = new Fixture();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.True(fixture.ResidueCollectors.Any(b => relayType.IsAssignableFrom(b.GetType())));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(ListSpecification), typeof(EnumerableFavoringConstructorQuery))]
        [InlineData(typeof(HashSetSpecification), typeof(EnumerableFavoringConstructorQuery))]
        [InlineData(typeof(CollectionSpecification), typeof(ListFavoringConstructorQuery))]
        public void CustomizeAddsBuilderForProperConcreteMultipleType(Type specificationType, Type queryType)
        {
            // Fixture setup
            var sut = new MultipleCustomization();
            var fixture = new Fixture();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.True(fixture.Customizations
                .OfType<FilteringSpecimenBuilder>()
                .Where(b => specificationType.IsAssignableFrom(b.Specification.GetType()))
                .Where(b => typeof(MethodInvoker).IsAssignableFrom(b.Builder.GetType()))
                .Select(b => (MethodInvoker)b.Builder)
                .Where(i => queryType.IsAssignableFrom(i.Query.GetType()))
                .Any());
            // Teardown
        }

        [Fact]
        public void CustomizeAddsBuilderForConcreteDictionaries()
        {
            // Fixture setup
            var sut = new MultipleCustomization();
            var fixture = new Fixture();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.True(fixture.Customizations
                .OfType<FilteringSpecimenBuilder>()
                .Where(b => typeof(DictionarySpecification).IsAssignableFrom(b.Specification.GetType()))
                .Where(b => typeof(Postprocessor).IsAssignableFrom(b.Builder.GetType()))
                .Select(b => (Postprocessor)b.Builder)
                .Where(p => p.Command is DictionaryFiller)
                .Where(p => typeof(MethodInvoker).IsAssignableFrom(p.Builder.GetType()))
                .Select(p => (MethodInvoker)p.Builder)
                .Where(i => typeof(ModestConstructorQuery).IsAssignableFrom(i.Query.GetType()))
                .Any());
            // Teardown
        }

        #region Usage/scenario tests

        [Fact]
        public void CreateAnonymousEnumerableReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.Create<IEnumerable<Version>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousListReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.Create<List<long>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousHashSetReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.Create<HashSet<string>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousIListReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.Create<IList<DateTime>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousICollectionReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.Create<ICollection<TimeSpan>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousCollectionReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.Create<Collection<Guid>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousDictionaryReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.Create<Dictionary<string, ConcreteType>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousIDictionaryReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.Create<IDictionary<float, object>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        #endregion
    }
}
