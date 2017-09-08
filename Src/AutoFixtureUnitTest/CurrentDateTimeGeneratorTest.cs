using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class CurrentDateTimeGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new CurrentDateTimeGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new CurrentDateTimeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Fixture setup
            var sut = new CurrentDateTimeGenerator();
            // Exercise system and verify outcome
            var dummyRequest = new object();
            Assert.Null(Record.Exception(() => sut.Create(dummyRequest, null)));
            // Teardown
        }

        [Fact]
        public void CreateWithNonDateTimeRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonDateTimeRequest = new object();
            var sut = new CurrentDateTimeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonDateTimeRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsCorrectResult()
        {
            // Fixture setup
            var before = DateTime.Now;
            var dateTimeRequest = typeof(DateTime);
            var sut = new CurrentDateTimeGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dateTimeRequest, dummyContext);
            // Verify outcome
            var after = DateTime.Now;
            var dt = Assert.IsAssignableFrom<DateTime>(result);
            Assert.True(before <= dt && dt <= after);
            // Teardown
        }
    }
}