﻿using System.Linq;
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
            // Arrange
            var resultResolver = Substitute.For<ICallResultResolver>();
            var resultsCache = Substitute.For<ICallResultCache>();
            var callSpecificationFactory = Substitute.For<ICallSpecificationFactory>();

            // Act
            var sut = new AutoFixtureValuesHandler(resultResolver, resultsCache, callSpecificationFactory);

            // Assert
            Assert.Equal(resultResolver, sut.ResultResolver);
            Assert.Equal(resultsCache, sut.ResultCache);
            Assert.Equal(callSpecificationFactory, sut.CallSpecificationFactory);
        }

        [Fact]
        public void ShouldUseCorrectSpecForValueCaching()
        {
            // Arrange
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callSpec = Substitute.For<ICallSpecification>();
            sut.CallSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall).Returns(callSpec);
            sut.ResultResolver.ResolveResult(call).Returns(
                new CallResultData(Maybe.Nothing<object>(), Enumerable.Empty<CallResultData.ArgumentValue>()));

            // Act
            sut.Handle(call);

            // Assert
            sut.ResultCache.Received().AddResult(callSpec, Arg.Any<CallResultData>());
        }

        [Fact]
        public void ShouldCacheResolvedValue()
        {
            // Arrange
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callResult =
                new CallResultData(Maybe.Nothing<object>(), Enumerable.Empty<CallResultData.ArgumentValue>());
            sut.ResultResolver.ResolveResult(call).Returns(callResult);

            // Act
            sut.Handle(call);

            // Assert
            sut.ResultCache.Received().AddResult(Arg.Any<ICallSpecification>(), callResult);
        }

        [Fact]
        public void ShouldReturnValueFromCacheIfPresent()
        {
            // Arrange
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callSpec = Substitute.For<ICallSpecification>();
            sut.CallSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall).Returns(callSpec);

            var cachedResult = "cachedResult";

            CallResultData ignored;
            sut.ResultCache.TryGetResult(call, out ignored)
                .Returns(c =>
                {
                    c[1] = new CallResultData(Maybe.Just<object>(cachedResult),
                        Enumerable.Empty<CallResultData.ArgumentValue>());
                    return true;
                });

            // Act
            var actualResult = sut.Handle(call);

            // Assert
            sut.ResultResolver.DidNotReceive().ResolveResult(Arg.Any<ICall>());
            Assert.True(actualResult.HasReturnValue);
            Assert.Equal(cachedResult, actualResult.ReturnValue);
        }

        [Fact]
        public void ShouldReturnResolvedValue()
        {
            // Arrange
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var callResult = "callResult";
            sut.ResultResolver.ResolveResult(call).Returns(new CallResultData(
                Maybe.Just<object>(callResult),
                Enumerable.Empty<CallResultData.ArgumentValue>()));

            // Act
            var actualResult = sut.Handle(call);

            // Assert
            Assert.True(actualResult.HasReturnValue);
            Assert.Equal(callResult, actualResult.ReturnValue);
        }

        [Fact]
        public void ShouldSetRefArgumentValue()
        {
            // Arrange
            var sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithRefIntMethod>();
            int ignored = 0;
            var call = CallHelper.CreateCallMock(() => target.Method(ref ignored));

            var callResult = new CallResultData(
                Maybe.Nothing<object>(),
                new[] { new CallResultData.ArgumentValue(0, 42) });

            sut.ResultResolver.ResolveResult(call).Returns(callResult);

            // Act
            sut.Handle(call);

            // Assert
            Assert.Equal(42, call.GetArguments()[0]);
        }

        [Fact]
        public void ShouldSetOutArgumentValue()
        {
            // Arrange
            var sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithOutVoidMethod>();
            int ignored;
            var call = CallHelper.CreateCallMock(() => target.Method(out ignored));

            var callResult = new CallResultData(
                Maybe.Nothing<object>(),
                new[] { new CallResultData.ArgumentValue(0, 42) });

            sut.ResultResolver.ResolveResult(call).Returns(callResult);

            // Act
            sut.Handle(call);

            // Assert
            Assert.Equal(42, call.GetArguments()[0]);
        }

        [Fact]
        public void ShouldNotUpdateModifiedRefArgument()
        {
            // Arrange
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
                        new[] { new CallResultData.ArgumentValue(0, 84) }));

            // Act
            sut.Handle(call);

            // Assert
            Assert.Equal(42, callArgs[0]);
        }

        [Fact]
        public void ShouldNotUpdateModifiedOutArgument()
        {
            // Arrange
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
                        new[] { new CallResultData.ArgumentValue(0, 84) }));

            // Act
            sut.Handle(call);

            // Assert
            Assert.Equal(42, call.GetArguments()[0]);
        }

        [Fact]
        public void ShouldNotSetPropertyIfResultNotResolved()
        {
            // Arrange
            AutoFixtureValuesHandler sut = CreateSutWithMockedDependencies();

            var target = Substitute.For<IInterfaceWithProperty>();
            var call = CallHelper.CreatePropertyGetterCallMock(() => target.Property);

            sut.ResultResolver.ResolveResult(call).Returns(
                new CallResultData(Maybe.Nothing<object>(), Enumerable.Empty<CallResultData.ArgumentValue>()));

            // Act
            var result = sut.Handle(call);

            // Assert
            Assert.False(result.HasReturnValue);
        }
    }
}