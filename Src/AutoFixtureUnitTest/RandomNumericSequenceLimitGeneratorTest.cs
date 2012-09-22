using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomNumericSequenceLimitGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomNumericSequenceLimitGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new RandomNumericSequenceLimitGenerator();
            // Exercise system
            var result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Fixture setup
            var dummyRequest = new object();
            var sut = new RandomNumericSequenceLimitGenerator();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithNonRandomNumericSequenceLimitRequestReturnsCorrectResult()
        {
            // Fixture setup
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            var expectedResult = new NoSpecimen(dummyRequest);
            var sut = new RandomNumericSequenceLimitGenerator();
            // Exercise system
            var result = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithRandomNumericSequenceLimitRequestReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(RandomNumericSequenceLimit);
            var dummyContext = new DelegatingSpecimenContext();
            var expectedResult = new RandomNumericSequenceLimit();
            var sut = new RandomNumericSequenceLimitGenerator();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
