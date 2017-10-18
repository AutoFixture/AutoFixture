using AutoFixture.AutoNSubstitute.CustomCallHandler;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public class CallResultCacheFactoryTest
    {
        [Fact]
        public void SetupReturnsValueOfCorrectType()
        {
            // Fixture setup
            var sut = new CallResultCacheFactory();

            // Exercise system
            var result = sut.CreateCache();

            // Verify outcome
            Assert.IsType<CallResultCache>(result);
            // Teardown
        }
    }
}