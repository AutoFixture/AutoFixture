using NSubstitute.Core;
using Ploeh.AutoFixture.AutoNSubstitute.CustomCallHandler;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public class CallResultDataTest
    {
        [Fact]
        public void ConstructorShouldSetCorrectProperties()
        {
            // Fixture setup
            var retValue = Maybe.Just(new object());
            var argumentValues = new CallResultData.ArgumentValue[1];

            // Exercise system
            var sut = new CallResultData(retValue, argumentValues);

            // Verify outcome
            Assert.Equal(retValue, sut.ReturnValue);
            Assert.Same(argumentValues, sut.ArgumentValues);

            // Teardown
        }
    }
}