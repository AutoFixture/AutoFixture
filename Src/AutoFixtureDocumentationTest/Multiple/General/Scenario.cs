using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using System.Collections.ObjectModel;
using Ploeh.AutoFixture.Kernel;

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
                fixture.CreateAnonymous<IEnumerable<int>>();
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
            var list = fixture.CreateAnonymous<List<int>>();
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
            var list = fixture.CreateAnonymous<IList<int>>();
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
                fixture.CreateAnonymous<Collection<int>>();
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
            var integers = fixture.CreateAnonymous<IEnumerable<int>>();
            // Verify outcome
            Assert.Equal(fixture.RepeatCount, integers.Count());
            // Teardown
        }

        [Fact]
        public void ManyIsUniqueByDefault()
        {
            var fixture = new Fixture();
            var expected = fixture.CreateMany<string>();
            Assert.False(expected.SequenceEqual(expected));
        }

        [Fact]
        public void EnumerablesAreUniqueByDefault()
        {
            var fixture = new Fixture()
                .Customize(new MultipleCustomization());
            var expected =
                fixture.CreateAnonymous<IEnumerable<string>>();
            Assert.False(expected.SequenceEqual(expected));
        }

        [Fact]
        public void ManyIsStableWithCustomization()
        {
            var fixture = new Fixture();
            var stableRelay = new StableFiniteSequenceRelay();
            fixture.Customizations.Add(stableRelay);

            var expected =
                fixture.CreateMany<string>();
            Assert.True(expected.SequenceEqual(expected));
        }

        [Fact]
        public void EnumerablesAreStableWithCustomization()
        {
            var fixture = new Fixture()
                .Customize(new StableMultipeCustomization());
            var expected =
                fixture.CreateAnonymous<IEnumerable<string>>();
            Assert.True(expected.SequenceEqual(expected));
        }
    }
}
