using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MultipleToEnumerableRelayTests
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new MultipleToEnumerableRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(object))]
        [InlineData(typeof(Type))]
        [InlineData(1)]
        [InlineData(9992)]
        [InlineData("")]
        public void CreateFromNonMultipleRequestReturnsCorrectResult(
            object request)
        {
            // Fixture setup
            var sut = new MultipleToEnumerableRelay();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen(request);
            Assert.Equal(expected, actual);
            // Teardown
        }
    }
}
