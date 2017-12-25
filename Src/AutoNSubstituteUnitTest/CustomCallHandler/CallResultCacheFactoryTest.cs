using AutoFixture.AutoNSubstitute.CustomCallHandler;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public class CallResultCacheFactoryTest
    {
        [Fact]
        public void SetupReturnsValueOfCorrectType()
        {
            // Arrange
            var sut = new CallResultCacheFactory();

            // Act
            var result = sut.CreateCache();

            // Assert
            Assert.IsType<CallResultCache>(result);
        }
    }
}