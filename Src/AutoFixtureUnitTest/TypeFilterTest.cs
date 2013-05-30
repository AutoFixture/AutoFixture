using System;

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class TypeFilterTest
    {
        [Theory]
        [InlineData(typeof(object), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(AbstractType), false)]
        [InlineData(typeof(IInterface), false)]
        [InlineData(typeof(StructType), true)]
        [InlineData(typeof(char), false)]
        [InlineData(typeof(decimal), true)]
        [InlineData(typeof(ActivityScope), false)]
        [InlineData(typeof(Nullable<int>), true)]
        public void IsCustomStructureIsSatisfiedByReturnsCorrectResult(Type request, bool expectedResult)
        {
            // Fixture setup
            // Exercise system
            var result = request.IsStructure();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }  
    }
}