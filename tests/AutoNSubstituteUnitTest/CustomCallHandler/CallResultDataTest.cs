using AutoFixture.AutoNSubstitute.CustomCallHandler;
using NSubstitute.Core;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public class CallResultDataTest
    {
        [Fact]
        public void ConstructorShouldSetCorrectProperties()
        {
            // Arrange
            var retValue = Maybe.Just(new object());
            var argumentValues = new CallResultData.ArgumentValue[1];

            // Act
            var sut = new CallResultData(retValue, argumentValues);

            // Assert
            Assert.Equal(retValue, sut.ReturnValue);
            Assert.Same(argumentValues, sut.ArgumentValues);
        }
    }
}