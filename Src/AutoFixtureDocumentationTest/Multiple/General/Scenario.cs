using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Multiple.General
{
    public class Scenario
    {
        [Fact]
        public void CreateAnonymousEnumerableReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture()
                .Customize(new MultipleCustomization());
            // Exercise system
            var integers =
                fixture.Create<IEnumerable<int>>();
            // Verify outcome
            Assert.True(integers.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousListReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var list = fixture.Create<List<int>>();
            // Verify outcome
            Assert.True(list.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousIListReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var list = fixture.Create<IList<int>>();
            // Verify outcome
            Assert.True(list.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousCollectionReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var collection =
                fixture.Create<Collection<int>>();
            // Verify outcome
            Assert.True(collection.Any());
            // Teardown
        }

        [Fact]
        public void CreateEnumerableWithCustomCount()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            fixture.RepeatCount = 10;
            // Exercise system
            var integers = fixture.Create<IEnumerable<int>>();
            // Verify outcome
            Assert.Equal(fixture.RepeatCount, integers.Count());
            // Teardown
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
