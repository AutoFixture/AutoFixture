using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class BoundaryConventionExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Fixture setup
            // Exercise system
            var sut = new BoundaryConventionException();
            // Verify outcome
            Assert.IsAssignableFrom<Exception>(sut);
            // Teardown
        }

        [Fact]
        public void MessageIsNotNull()
        {
            // Fixture setup
            var sut = new BoundaryConventionException();
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void MessageIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = fixture.CreateAnonymous("Message");

            var sut = new BoundaryConventionException(expected);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.Equal<string>(expected, result);
            // Teardown
        }

        [Fact]
        public void InnerExceptionIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = fixture.CreateAnonymous<Exception>();

            var sut = new BoundaryConventionException(fixture.CreateAnonymous("Message"), expected);
            // Exercise system
            var result = sut.InnerException;
            // Verify outcome
            Assert.Equal<Exception>(expected, result);
            // Teardown
        }
    }
}