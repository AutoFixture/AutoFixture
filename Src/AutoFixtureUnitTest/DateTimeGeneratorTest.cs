using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DateTimeGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new DateTimeGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new DateTimeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrowsArgumentNullException()
        {
            // Fixture setup
            var sut = new DateTimeGenerator();
            // Exercise system and verify outcome
            var dummyRequest = new object();
            Assert.Throws(typeof(ArgumentNullException), () => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithNonDateTimeRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonDateTimeRequest = new object();
            var sut = new DateTimeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonDateTimeRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonDateTimeRequest);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsDateTimeValue()
        {
            // Fixture setup
            var dateTimeRequest = typeof(DateTime);
            var sut = new DateTimeGenerator();
            // Exercise system
            var context = new DelegatingSpecimenContext { OnResolve = r => 1 };
            var result = sut.Create(dateTimeRequest, context);
            // Verify outcome
            Assert.IsAssignableFrom<DateTime>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithDateTimeRequestReturnsDifferentDay()
        {
            // Fixture setup
            var today = DateTime.Today;
            var dateTimeRequest = typeof(DateTime);
            var sut = new DateTimeGenerator();
            // Exercise system
            var context = new DelegatingSpecimenContext { OnResolve = r => 1 };
            var result = sut.Create(dateTimeRequest, context);
            // Verify outcome
            Assert.NotEqual(today, ((DateTime)result).Date);
            // Teardown
        }
    }
}