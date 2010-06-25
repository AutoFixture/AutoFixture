using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;
using Ploeh.AutoFixtureUnitTest.Kernel;

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
        public void CreateWithNullContextDoesNotThrow()
        {
            // Fixture setup
            var sut = new DateTimeGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
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
        public void CreateWithDateTimeRequestReturnsCorrectResult()
        {
            // Fixture setup
            var before = DateTime.Now;
            var dateTimeRequest = typeof(DateTime);
            var sut = new DateTimeGenerator();
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
