using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomBooleanSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomBooleanSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new RandomBooleanSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Fixture setup
            var sut = new RandomBooleanSequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonBooleanRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonBooleanRequest = new object();
            var sut = new RandomBooleanSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonBooleanRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonBooleanRequest);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithBooleanRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var booleanRequest = typeof(bool);
            var sut = new RandomBooleanSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(booleanRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<bool>(result);
            // Teardown
        } 
    }
}