using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SeedRequestSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            var dummyType = typeof(object);
            // Exercise system
            var sut = new SeedRequestSpecification(dummyType);
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new SeedRequestSpecification(null));
            // Teardown
        }

        [Fact]
        public void TargetTypeIsCorrect()
        {
            // Fixture setup
            var expectedType = typeof(DayOfWeek);
            var sut = new SeedRequestSpecification(expectedType);
            // Exercise system
            Type result = sut.TargetType;
            // Verify outcome
            Assert.Equal(expectedType, result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var sut = new SeedRequestSpecification(dummyType);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByNonSeedReturnsCorrectResult()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var sut = new SeedRequestSpecification(dummyType);
            var nonSeedRequest = new object();
            // Exercise system
            var result = sut.IsSatisfiedBy(nonSeedRequest);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object), typeof(object), true)]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(string), typeof(int), false)]
        [InlineData(typeof(PropertyHolder<string>), typeof(FieldHolder<string>), false)]
        public void IsSatisfiedByReturnsCorrectResult(Type specType, Type seedRequestType, bool expectedResult)
        {
            // Fixture setup
            var sut = new SeedRequestSpecification(specType);
            var dummySeed = new object();
            var seededRequest = new SeededRequest(seedRequestType, dummySeed);
            // Exercise system
            var result = sut.IsSatisfiedBy(seededRequest);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
