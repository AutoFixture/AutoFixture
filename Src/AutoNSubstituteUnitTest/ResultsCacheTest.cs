using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class ResultsCacheTest
    {
        [Fact]
        public void AddedResultIsReturned()
        {
            //arrange
            var sut = new ResultsCache();
            var cachedResult = new CachedCallResult(null, null);

            var call = Substitute.For<ICall>();
            var callSpec = Substitute.For<ICallSpecification>();
            callSpec.IsSatisfiedBy(call).Returns(true);

            //act
            sut.AddResult(callSpec, cachedResult);

            CachedCallResult retrievedResult;
            var hasResult = sut.TryGetResult(call, out retrievedResult);

            //assert
            Assert.True(hasResult);
            Assert.Same(cachedResult, retrievedResult);
        }

        [Fact]
        public void TheLatestResultIsReturned()
        {
            //arrange
            var sut = new ResultsCache();
            var cachedResult = new CachedCallResult(null, null);

            var call = Substitute.For<ICall>();
            var callSpec1 = Substitute.For<ICallSpecification>();
            callSpec1.IsSatisfiedBy(call).Returns(true);

            var callSpec2 = Substitute.For<ICallSpecification>();
            callSpec2.IsSatisfiedBy(call).Returns(true);

            //act
            sut.AddResult(callSpec1, new CachedCallResult(null, null));
            sut.AddResult(callSpec2, cachedResult);

            CachedCallResult retrievedResult;
            var hasResult = sut.TryGetResult(call, out retrievedResult);

            //assert
            Assert.True(hasResult);
            Assert.Same(cachedResult, retrievedResult);
        }

        [Fact]
        public void DoesntReturnIfSpecificationIsNotSatisfied()
        {
            //arrange
            var sut = new ResultsCache();

            var call = Substitute.For<ICall>();
            var callSpec = Substitute.For<ICallSpecification>();
            callSpec.IsSatisfiedBy(call).Returns(false);

            var cachedResult = new CachedCallResult(null, null);

            //act
            sut.AddResult(callSpec, cachedResult);

            CachedCallResult retrievedResult;
            var hasResult = sut.TryGetResult(call, out retrievedResult);

            //assert
            Assert.False(hasResult);
            Assert.Null(retrievedResult);
        }
    }
}