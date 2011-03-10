using System;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReferenceTypeBoundaryConventionTest
    {
        [Fact]
        public void SutIsBoundaryConvention()
        {
            // Fixture setup
            // Exercise system
            var sut = new ReferenceTypeBoundaryConvention();
            // Verify outcome
            Assert.IsAssignableFrom<IBoundaryConvention>(sut);
            // Teardown
        }

        [Fact]
        public void CreateBoundaryBehaviorsWithNullTypeThrows()
        {
            // Fixture setup
            var sut = new ReferenceTypeBoundaryConvention();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.CreateBoundaryBehaviors(null).ToList());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(OperatingSystem))]
        [InlineData(typeof(IAsyncResult))]
        public void CreateBoundaryBehaviorsForReferenceTypeReturnsCorrectResult(Type referenceType)
        {
            // Fixture setup
            var sut = new ReferenceTypeBoundaryConvention();
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(referenceType);
            // Verify outcome
            Assert.True(result.OfType<NullReferenceBehavior>().Any());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(DateTime))]
        public void CreateBoundaryBehaviorsForValueTypeReturnsCorrectResult(Type valueType)
        {
            // Fixture setup
            var sut = new ReferenceTypeBoundaryConvention();
            // Exercise system
            var result = sut.CreateBoundaryBehaviors(valueType);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }
    }
}
