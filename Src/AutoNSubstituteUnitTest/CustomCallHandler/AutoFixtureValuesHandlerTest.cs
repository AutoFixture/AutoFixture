using System.Linq;
using AutoFixture.AutoNSubstitute.CustomCallHandler;
using AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public class AutoFixtureValuesHandlerTest
    {
        private static AutoFixtureValuesHandler CreateSutWithMockedDependencies()
        {
            var resultResolver = Substitute.For<ICallResultResolver>();
            var resultsCache = Substitute.For<ICallResultCache>();
            var callSpecificationFactory = Substitute.For<ICallSpecificationFactory>();

            return new AutoFixtureValuesHandler(resultResolver, resultsCache, callSpecificationFactory);
        }

        [Fact]
        public void ShouldSetConstructorValuesToProperties()
        {
            // Fixture setup
            var resultResolver = Substitute.For<ICallResultResolver>();
            var resultsCache = Substitute.For<ICallResultCache>();
            var callSpecificationFactory = Substitute.For<ICallSpecificationFactory>();

            // Exercise system
            var sut = new AutoFixtureValuesHandler(resultResolver, resultsCache, callSpecificationFactory);

            // Verify outcome
            Assert.Equal(resultResolver, sut.ResultResolver);
            Assert.Equal(resultsCache, sut.ResultCache);
            Assert.Equal(callSpecificationFactory, sut.CallSpecificationFactory);

            // Teardown
        }

        [Fact]
        public void ShouldUseCorrectSpecForValueCaching()
        {
            // Fixture setup
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callSpec = Substitute.For<ICallSpecification>();
            sut.CallSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall).Returns(callSpec);
            sut.ResultResolver.ResolveResult(call).Returns(
                new CallResultData(Maybe.Nothing<object>(), Enumerable.Empty<CallResultData.ArgumentValue>()));

            // Exercise system
            sut.Handle(call);

            // Verify outcome
            sut.ResultCache.Received().AddResult(callSpec, Arg.Any<CallResultData>());

            // Teardown
        }

        [Fact]
        public void ShouldCacheResolvedValue()
        {
            // Fixture setup
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callResult =
                new CallResultData(Maybe.Nothing<object>(), Enumerable.Empty<CallResultData.ArgumentValue>());
            sut.ResultResolver.ResolveResult(call).Returns(callResult);

            // Exercise system
            sut.Handle(call);

            // Verify outcome
            sut.ResultCache.Received().AddResult(Arg.Any<ICallSpecification>(), callResult);

            // Teardown
        }

        [Fact]
        public void ShouldReturnValueFromCacheIfPresent()
        {
            // Fixture setup
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callSpec = Substitute.For<ICallSpecification>();
            sut.CallSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall).Returns(callSpec);

            var cachedResult = "cachedResult";

            CallResultData _;
            sut.ResultCache.TryGetResult(call, out _)
                .Returns(c =>
                {
                    c[1] = new CallResultData(Maybe.Just<object>(cachedResult),
                        Enumerable.Empty<CallResultData.ArgumentValue>());
                    return true;
                });

            // Exercise system
            var actualResult = sut.Handle(call);

            // Verify outcome
            sut.ResultResolver.DidNotReceive().ResolveResult(Arg.Any<ICall>());
            Assert.True(actualResult.HasReturnValue);
            Assert.Equal(cachedResult, actualResult.ReturnValue);

            // Teardown
        }

        [Fact]
        public void ShouldReturnResolvedValue()
        {
            // Fixture setup
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callResult = "callResult";
            sut.ResultResolver.ResolveResult(call).Returns(new CallResultData(
                Maybe.Just<object>(callResult),
                Enumerable.Empty<CallResultData.ArgumentValue>()));

            // Exercise system
            var actualResult = sut.Handle(call);

            // Verify outcome
            Assert.True(actualResult.HasReturnValue);
            Assert.Equal(callResult, actualResult.ReturnValue);

            // Teardown
        }

        [Fact]
        public void ShouldSetRefArgumentValue()
        {
            // Fixture setup
            var sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithRefIntMethod>();
            int _ = 0;
            var call = CallHelper.CreateCallMock(() => target.Method(ref _));

            var callResult = new CallResultData(
                Maybe.Nothing<object>(),
                new[] {new CallResultData.ArgumentValue(0, 42)});

            sut.ResultResolver.ResolveResult(call).Returns(callResult);

            // Exercise system
            sut.Handle(call);

            // Verify outcome
            Assert.Equal(42, call.GetArguments()[0]);

            // Teardown
        }

        [Fact]
        public void ShouldSetOutArgumentValue()
        {
            // Fixture setup
            var sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithOutVoidMethod>();
            int _;
            var call = CallHelper.CreateCallMock(() => target.Method(out _));

            var callResult = new CallResultData(
                Maybe.Nothing<object>(),
                new[] {new CallResultData.ArgumentValue(0, 42)});

            sut.ResultResolver.ResolveResult(call).Returns(callResult);

            // Exercise system
            sut.Handle(call);

            // Verify outcome
            Assert.Equal(42, call.GetArguments()[0]);

            // Teardown
        }


        [Fact]
        public void ShouldNotUpdateModifiedRefArgument()
        {
            // Fixture setup
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithRefVoidMethod>();

            int origValue = 10;
            var call = CallHelper.CreateCallMock(() => target.Method(ref origValue));
            call.GetArguments()[0] = 42;

            var callArgs = call.GetArguments();

            sut.ResultResolver
                .ResolveResult(call)
                .Returns(
                    new CallResultData(
                        Maybe.Nothing<object>(),
                        new[] {new CallResultData.ArgumentValue(0, 84)}));

            // Exercise system
            sut.Handle(call);

            // Verify outcome
            Assert.Equal(42, callArgs[0]);

            // Teardown
        }

        [Fact]
        public void ShouldNotUpdateModifiedOutArgument()
        {
            // Fixture setup
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithOutVoidMethod>();

            int origValue;
            var call = CallHelper.CreateCallMock(() => target.Method(out origValue));
            call.GetArguments()[0] = 42;


            sut.ResultResolver
                .ResolveResult(call)
                .Returns(
                    new CallResultData(
                        Maybe.Nothing<object>(),
                        new[] {new CallResultData.ArgumentValue(0, 84)}));

            // Exercise system
            sut.Handle(call);

            // Verify outcome
            Assert.Equal(42, call.GetArguments()[0]);

            // Teardown
        }

        [Fact]
        public void ShouldNotSetPropertyIfResultNotResolved()
        {
            // Fixture setup
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithProperty>();
            var call = CallHelper.CreatePropertyGetterCallMock(() => target.Property);

            sut.ResultResolver.ResolveResult(call).Returns(
                new CallResultData(Maybe.Nothing<object>(), Enumerable.Empty<CallResultData.ArgumentValue>()));

            // Exercise system
            var result = sut.Handle(call);

            // Verify outcome
            Assert.False(result.HasReturnValue);

            // Teardown
        }
    }
}