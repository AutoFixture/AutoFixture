using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class StructureSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new StructureSpecification();
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Fixture setup
            var sut = new StructureSpecification();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Theory]
        [InlineData("Ploeh", false)]
        [InlineData(1, false)]
        [InlineData(typeof(object), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(AbstractType), false)]
        [InlineData(typeof(IInterface), false)]
        [InlineData(typeof(StructType), true)]
        [InlineData(typeof(char), false)]
        [InlineData(typeof(ActivityScope), false)]
        [InlineData(typeof(decimal), true)]
        [InlineData(typeof(Nullable<int>), true)]
        public void IsSatisfiedByReturnsCorrectResult(object request, bool expectedResult)
        {
            // Fixture setup
            var sut = new StructureSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        } 
    }
}