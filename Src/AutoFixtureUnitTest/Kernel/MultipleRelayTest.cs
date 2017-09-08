using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MultipleRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new MultipleRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CountIsProperWritableProperty()
        {
            // Fixture setup
            var sut = new MultipleRelay();
            var expectedCount = 1;
            // Exercise system
            sut.Count = expectedCount;
            int result = sut.Count;
            // Verify outcome
            Assert.Equal(expectedCount, result);
            // Teardown
        }

        [Fact]
        public void DefaultCountIsCorrect()
        {
            // Fixture setup
            var sut = new MultipleRelay();
            // Exercise system
            var result = sut.Count;
            // Verify outcome
            Assert.Equal(3, result);
            // Teardown
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-21)]
        public void SettingInvalidCountThrows(int count)
        {
            // Fixture setup
            var sut = new MultipleRelay();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                sut.Count = count);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new MultipleRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new MultipleRelay();
            var request = new object();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("foo")]
        public void CreateWithInvalidRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new MultipleRelay();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithManyRequestReturnsCorrectResult()
        {
            // Fixture setup
            var request = new MultipleRequest(new object());
            var count = 7;
            var expectedTranslation = new FiniteSequenceRequest(request.Request, 7);
            var expectedResult = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedTranslation.Equals(r) ? expectedResult : new NoSpecimen() };

            var sut = new MultipleRelay { Count = count };
            // Exercise system
            var result = sut.Create(request, container);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
