using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Multiple.Specific
{
    public class Scenario
    {
        [Fact]
        public void UncustomizedListReturnsPopulatedList()
        {
            var fixture = new Fixture();
            var list = fixture.Create<List<int>>();
            Assert.True(list.Any());
        }

        [Fact]
        public void PopulateListAfterCreation()
        {
            var fixture = new Fixture();
            var list = fixture.Create<List<int>>();
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
                fixture.Create<IEnumerable<int>>();
            Assert.True(integers.Any());
        }

        [Fact]
        public void CreatePopulatedList()
        {
            var fixture = new Fixture();
            fixture.Register(() =>
                fixture.CreateMany<int>().ToList());
            var list = fixture.Create<List<int>>();
            Assert.True(list.Any());
        }

        [Fact]
        public void CreatePopulatedCollection()
        {
            var fixture = new Fixture();
            fixture.Register(() =>
                new Collection<int>(
                    fixture.CreateMany<int>().ToList()));
            var collection = fixture.Create<Collection<int>>();
            Assert.True(collection.Any());
        }

        [Fact]
        public void CreatePopulatedListInterface()
        {
            var fixture = new Fixture();
            fixture.Register<IList<int>>(() => 
                fixture.CreateMany<int>().ToList());
            var list = fixture.Create<IList<int>>();
            Assert.True(list.Any());
        }
    }
}
