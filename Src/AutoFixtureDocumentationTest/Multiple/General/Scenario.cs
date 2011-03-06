using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using System.Collections.ObjectModel;

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
    }
}
