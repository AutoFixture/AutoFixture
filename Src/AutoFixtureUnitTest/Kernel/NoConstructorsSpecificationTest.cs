using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class NoConstructorsSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new NoConstructorsSpecification();
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Fixture setup
            var sut = new NoConstructorsSpecification();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Theory]
        [InlineData("Ploeh", false)]
        [InlineData(1, false)]
        [InlineData(typeof(object), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(AbstractType), true)]
        [InlineData(typeof(IInterface), true)]
        [InlineData(typeof(MutableValueType), false)]
        [InlineData(typeof(char), true)]
        [InlineData(typeof(ActivityScope), true)]
        public void IsSatisfiedByReturnsCorrectResult(object request, bool expectedResult)
        {
            // Fixture setup
            var sut = new NoConstructorsSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }  
    }
}