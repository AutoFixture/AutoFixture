using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    [Obsolete]
    public class MultipleCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            // Act
            var sut = new MultipleCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var sut = new MultipleCustomization();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Theory]
        [InlineData(typeof(EnumerableRelay))]
        [InlineData(typeof(ListRelay))]
        [InlineData(typeof(CollectionRelay))]
        [InlineData(typeof(DictionaryRelay))]
        public void CustomizeAddsRelayToFixture(Type relayType)
        {
            // Arrange
            var sut = new MultipleCustomization();
            var fixture = new Fixture();
            // Act
            sut.Customize(fixture);
            // Assert
            Assert.Contains(fixture.ResidueCollectors, relayType.IsInstanceOfType);
        }

        [Theory]
        [InlineData(typeof(ListSpecification), typeof(EnumerableFavoringConstructorQuery))]
        [InlineData(typeof(HashSetSpecification), typeof(EnumerableFavoringConstructorQuery))]
        [InlineData(typeof(CollectionSpecification), typeof(ListFavoringConstructorQuery))]
        public void CustomizeAddsBuilderForProperConcreteMultipleType(Type specificationType, Type queryType)
        {
            // Arrange
            var sut = new MultipleCustomization();
            var fixture = new Fixture();
            // Act
            sut.Customize(fixture);
            // Assert
            Assert.True(fixture.Customizations
                .OfType<FilteringSpecimenBuilder>()
                .Where(b => specificationType.IsAssignableFrom(b.Specification.GetType()))
                .Where(b => typeof(MethodInvoker).IsAssignableFrom(b.Builder.GetType()))
                .Select(b => (MethodInvoker)b.Builder)
                .Where(i => queryType.IsAssignableFrom(i.Query.GetType()))
                .Any());
        }

        [Fact]
        public void CustomizeAddsBuilderForConcreteDictionaries()
        {
            // Arrange
            var sut = new MultipleCustomization();
            var fixture = new Fixture();
            // Act
            sut.Customize(fixture);
            // Assert
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
        }

        #region Usage/scenario tests

        [Fact]
        public void CreateAnonymousEnumerableReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Act
            var result = fixture.Create<IEnumerable<Version>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void CreateAnonymousListReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Act
            var result = fixture.Create<List<long>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void CreateAnonymousHashSetReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Act
            var result = fixture.Create<HashSet<string>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void CreateAnonymousIListReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Act
            var result = fixture.Create<IList<DateTime>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void CreateAnonymousICollectionReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Act
            var result = fixture.Create<ICollection<TimeSpan>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void CreateAnonymousCollectionReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Act
            var result = fixture.Create<Collection<Guid>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void CreateAnonymousDictionaryReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Act
            var result = fixture.Create<Dictionary<string, ConcreteType>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void CreateAnonymousIDictionaryReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Act
            var result = fixture.Create<IDictionary<float, object>>();
            // Assert
            Assert.True(result.Any());
        }

        #endregion
    }
}
