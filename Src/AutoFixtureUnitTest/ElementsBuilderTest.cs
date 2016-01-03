using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using System;
using System.Linq;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ElementsBuilderTest
    {
        [Fact]
        public void CreateWithNullShouldThrow()
        {
            // Fixture setup
            // Exercise system & Verify outcome
            Assert.Throws<ArgumentNullException>(() => new ElementsBuilder<int>(null));
            // Teardown
        }

        [Fact]
        public void CreateWithEmptyCollectionShouldThrow()
        {
            // Fixture setup
            // Exercise system & Verify outcome
            Assert.Throws<ArgumentException>(() => new ElementsBuilder<int>(Enumerable.Empty<int>()));
            // Teardown
        }

        [Fact]
        public void CreateWithZeroArgumentsShouldThrow()
        {
            // Fixture setup
            // Exercise system & Verify outcome
            Assert.Throws<ArgumentException>(() => new ElementsBuilder<int>());
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ElementsBuilder<int>(42);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNonCorrectTypeRequestWillReturnNoSpecimen()
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new ElementsBuilder<int>(42);
            // Exercise system
            var result = sut.Create(typeof(string), dummyContext);
            // Verify outcome
            Assert.True(result is NoSpecimen);
            // Teardown
        }

        [Fact]
        public void CreateWithCorrectTypeRequestWillReturnCorrectTypeSpecimen()
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new ElementsBuilder<int>(42);
            // Exercise system
            var result = sut.Create(typeof(int), dummyContext);
            // Verify outcome
            Assert.Equal(typeof(int), result.GetType());
            // Teardown
        }

        [Fact]
        public void CreateWithOneElementCollectionWillReturnThatElement()
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new ElementsBuilder<int>(42);
            // Exercise system
            var result = sut.Create(typeof(int), dummyContext);
            // Verify outcome
            Assert.Equal(42, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsElementFromTheCollection()
        {
            // Fixture setup
            var collection = new[] { "foo", "bar", "qux" };
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new ElementsBuilder<string>(collection);
            // Exercise system
            var result = sut.Create(typeof(string), dummyContext);
            // Verify outcome
            Assert.Contains(result, collection);
            // Teardown 
        }

        [Fact]
        public void CreateDoesNotReturnTheSameElementTwiceWhenCalledTwoTimesWithTwoElementsInCollection()
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new ElementsBuilder<int>(42, 7);
            // Exercise system
            var result1 = sut.Create(typeof(int), dummyContext);
            var result2 = sut.Create(typeof(int), dummyContext);
            // Verify outcome
            Assert.NotEqual(result1, result2);
            // Teardown 
        }
    }
}
