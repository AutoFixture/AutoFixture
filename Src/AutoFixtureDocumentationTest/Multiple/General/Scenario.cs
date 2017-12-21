using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureDocumentationTest.Multiple.General
{
    public class Scenario
    {
        [Fact]
        public void CreateAnonymousEnumerableReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var integers =
                fixture.Create<IEnumerable<int>>();
            // Assert
            Assert.True(integers.Any());
        }

        [Fact]
        public void CreateAnonymousListReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var list = fixture.Create<List<int>>();
            // Assert
            Assert.True(list.Any());
        }

        [Fact]
        public void CreateAnonymousIListReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var list = fixture.Create<IList<int>>();
            // Assert
            Assert.True(list.Any());
        }

        [Fact]
        public void CreateAnonymousCollectionReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var collection =
                fixture.Create<Collection<int>>();
            // Assert
            Assert.True(collection.Any());
        }

        [Fact]
        public void CreateEnumerableWithCustomCount()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.RepeatCount = 10;
            // Act
            var integers = fixture.Create<IEnumerable<int>>();
            // Assert
            Assert.Equal(fixture.RepeatCount, integers.Count());
        }

        [Fact]
        public void ManyIsStableByDefault()
        {
            var fixture = new Fixture();
            var expected = fixture.CreateMany<string>();
            Assert.True(expected.SequenceEqual(expected));
        }

        [Fact]
        public void EnumerablesAreStableByDefault()
        {
            var fixture = new Fixture();
            var expected =
                fixture.Create<IEnumerable<string>>();
            Assert.True(expected.SequenceEqual(expected));
        }

        [Fact]
        public void ManyCanBeMadeUniqueByRemovingCustomization()
        {
            var fixture = new Fixture();
            fixture
                .Customizations
                .OfType<StableFiniteSequenceRelay>()
                .ToList()
                .ForEach(c => fixture.Customizations.Remove(c));

            var expected =
                fixture.CreateMany<string>();
            Assert.False(expected.SequenceEqual(expected));
        }

        [Fact]
        public void EnumerablesCanBeMadeUniqueByRemovingCustomization()
        {
            var fixture = new Fixture();
            fixture
                .Customizations
                .OfType<StableFiniteSequenceRelay>()
                .ToList()
                .ForEach(c => fixture.Customizations.Remove(c));

            var expected =
                fixture.Create<IEnumerable<string>>();
            Assert.False(expected.SequenceEqual(expected));
        }
    }
}
