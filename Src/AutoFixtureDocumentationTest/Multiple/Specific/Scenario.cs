using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using System.Collections.ObjectModel;

namespace Ploeh.AutoFixtureDocumentationTest.Multiple.Specific
{
    public class Scenario
    {
        [Fact]
        public void UncustomizedListReturnsEmptyList()
        {
            var fixture = new Fixture();
            var list = fixture.CreateAnonymous<List<int>>();
            Assert.False(list.Any());
        }

        [Fact]
        public void PopulateListAfterCreation()
        {
            var fixture = new Fixture();
            var list = fixture.CreateAnonymous<List<int>>();
            fixture.AddManyTo(list);
            Assert.True(list.Any());
        }

        [Fact]
        public void CreateManyIntegers()
        {
            var fixture = new Fixture();
            var integers = fixture.CreateMany<int>();
            Assert.True(integers.Any());
        }

        [Fact]
        public void CreatePopulatedEnumerable()
        {
            var fixture = new Fixture();
            fixture.Register(() => fixture.CreateMany<int>());
            var integers =
                fixture.CreateAnonymous<IEnumerable<int>>();
            Assert.True(integers.Any());
        }

        [Fact]
        public void CreatePopulatedList()
        {
            var fixture = new Fixture();
            fixture.Register(() =>
                fixture.CreateMany<int>().ToList());
            var list = fixture.CreateAnonymous<List<int>>();
            Assert.True(list.Any());
        }

        [Fact]
        public void CreatePopulatedCollection()
        {
            var fixture = new Fixture();
            fixture.Register(() =>
                new Collection<int>(
                    fixture.CreateMany<int>().ToList()));
            var collection = fixture.CreateAnonymous<Collection<int>>();
            Assert.True(collection.Any());
        }

        [Fact]
        public void CreatePopulatedListInterface()
        {
            var fixture = new Fixture();
            fixture.Register<IList<int>>(() => 
                fixture.CreateMany<int>().ToList());
            var list = fixture.CreateAnonymous<IList<int>>();
            Assert.True(list.Any());
        }
    }
}
