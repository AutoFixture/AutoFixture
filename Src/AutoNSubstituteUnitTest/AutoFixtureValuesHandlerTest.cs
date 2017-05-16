using System;
using NSubstitute;
using NSubstitute.Core;
using Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class AutoFixtureValuesHandlerTest
    {
        private static readonly Tuple<int, object>[] Empty = new Tuple<int, object>[0];

        private readonly Fixture _fixture;
        private readonly ICallResultResolver _callResultResolver;
        private readonly IResultsCache _resultsCache;
        private readonly ICallSpecificationFactory _callSpecificationFactory;
        private readonly AutoFixtureValuesHandler _sut;

        public AutoFixtureValuesHandlerTest()
        {
            _fixture = new Fixture();
            _callResultResolver = Substitute.For<ICallResultResolver>();
            _resultsCache = Substitute.For<IResultsCache>();
            _callSpecificationFactory = Substitute.For<ICallSpecificationFactory>();

            _sut = new AutoFixtureValuesHandler(_callResultResolver, _resultsCache, _callSpecificationFactory);
        }

        [Fact]
        public void ResultValueIsCachedWithCorrectSpecs()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callSpec = Substitute.For<ICallSpecification>();
            _callSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall).Returns(callSpec);
            _callResultResolver.ResolveResult(call).Returns(new CallResultData(Maybe.Nothing<object>(), Empty));

            //act
            _sut.Handle(call);

            //assert
            _resultsCache.Received().AddResult(callSpec, Arg.Any<CallResultData>());
        }

        [Fact]
        public void ResolvedValueIsCached()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());


            var callResult = new CallResultData(Maybe.Nothing<object>(), Empty);
            _callResultResolver.ResolveResult(call).Returns(callResult);

            //act
            _sut.Handle(call);

            //assert
            _resultsCache.Received().AddResult(Arg.Any<ICallSpecification>(), callResult);
        }

        [Fact]
        public void ReturnsValueFromCacheIfPresent()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callSpec = Substitute.For<ICallSpecification>();
            _callSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall).Returns(callSpec);

            var cachedResult = _fixture.Create("stringRetValue");

            CallResultData _;
            _resultsCache.TryGetResult(call, out _)
                .Returns(c =>
                {
                    c[1] = new CallResultData(Maybe.Just<object>(cachedResult), Empty);
                    return true;
                });

            //act
            var actualResult = _sut.Handle(call);

            //assert
            _callResultResolver.DidNotReceive().ResolveResult(Arg.Any<ICall>());
            Assert.True(actualResult.HasReturnValue);
            Assert.Equal(cachedResult, actualResult.ReturnValue);
        }

        [Fact]
        public void ResolvedValueIsReturned()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callResult = _fixture.Create("callResult");
            _callResultResolver.ResolveResult(call).Returns(new CallResultData(Maybe.Just<object>(callResult), Empty));

            //act
            var actualResult = _sut.Handle(call);

            //assert
            Assert.True(actualResult.HasReturnValue);
            Assert.Equal(callResult, actualResult.ReturnValue);
        }

        [Fact]
        public void ArgumentIsSet()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithRefIntMethod>();
            int _ = 0;
            var call = CallHelper.CreateCallMock(() => target.Method(ref _));
            var callArgs = call.GetArguments();

            var intValue = _fixture.Create<int>();
            var callResult = new CallResultData(
                Maybe.Nothing<object>(),
                new[] {Tuple.Create(0, (object) intValue)});

            _callResultResolver.ResolveResult(call).Returns(callResult);

            //act
            _sut.Handle(call);

            //assert
            Assert.Equal(intValue, callArgs[0]);
        }


        [Fact]
        public void ModifiedArgumentIsNotUpdated()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithRefVoidMethod>();

            int origValue = 0;
            var call = CallHelper.CreateCallMock(() => target.Method(ref origValue), new object[] {100});
            var callArgs = call.GetArguments();

            var intValue = _fixture.Create<int>();
            _callResultResolver
                .ResolveResult(call)
                .Returns(
                    new CallResultData(
                        Maybe.Nothing<object>(),
                        new[] {Tuple.Create(0, (object) intValue)}));

            //act
            _sut.Handle(call);

            //assert
            Assert.Equal(origValue, callArgs[0]);
        }

        [Fact]
        public void PropertyIsNotSetIfNoResult()
        {
            var target = Substitute.For<IInterfaceWithProperty>();
            var call = CallHelper.CreatePropertyCallMock(() => target.Property);

            _callResultResolver.ResolveResult(call).Returns(new CallResultData(Maybe.Nothing<object>(), Empty));

            //act
            var result = _sut.Handle(call);

            //assert
            Assert.False(result.HasReturnValue);
        }
    }
}