using System;
using System.Collections.Generic;
using AutoFixture.AutoNSubstitute.CustomCallHandler;
using AutoFixture.Kernel;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Exceptions;
using NSubstitute.Routing;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class NSubstituteRegisterCallHandlerCommandTest
    {
        [Fact]
        public void ShouldReturnValuesFromConstructor()
        {
            // Arrange
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var callResultsCacheFactory = Substitute.For<ICallResultCacheFactory>();
            var callResultResolverFactory = Substitute.For<ICallResultResolverFactory>();

            // Act
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext, callResultsCacheFactory,
                callResultResolverFactory);

            // Assert
            Assert.Equal(substitutionContext, sut.SubstitutionContext);
            Assert.Equal(callResultsCacheFactory, sut.CallResultCacheFactory);
            Assert.Equal(callResultResolverFactory, sut.CallResultResolverFactory);
        }

        [Fact]
        public void ShouldUseConstructorDefaultsOfCorrectType()
        {
            // Arrange
            var substitutionContext = Substitute.For<ISubstitutionContext>();

            // Act
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext);

            // Assert
            Assert.IsType<CallResultCacheFactory>(sut.CallResultCacheFactory);
            Assert.IsType<CallResultResolverFactory>(sut.CallResultResolverFactory);
        }

        [Fact]
        public void ShouldUseValueFromCallResultsCacheFactory()
        {
            // Arrange
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var callResultsCacheFactory = Substitute.For<ICallResultCacheFactory>();
            var callResultResolverFactory = Substitute.For<ICallResultResolverFactory>();

            var sut = new NSubstituteRegisterCallHandlerCommand(
                substitutionContext, callResultsCacheFactory, callResultResolverFactory);

            var specimen = new object();
            var specimenContext = Substitute.For<ISpecimenContext>();
            var callRouter = new CallRouterStub();
            substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            var expectedResultsCache = Substitute.For<ICallResultCache>();
            callResultsCacheFactory.CreateCache().Returns(expectedResultsCache);

            // Act
            sut.Execute(specimen, specimenContext);

            // Assert
            var handler =
                (AutoFixtureValuesHandler) callRouter.RegisteredFactory.Invoke(Substitute.For<ISubstituteState>());
            Assert.Equal(expectedResultsCache, handler.ResultCache);
        }

        [Fact]
        public void ShouldUseValueFromCallResultResolverFactory()
        {
            // Arrange
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var callResultsCacheFactory = Substitute.For<ICallResultCacheFactory>();
            var callResultResolverFactory = Substitute.For<ICallResultResolverFactory>();

            var sut = new NSubstituteRegisterCallHandlerCommand(
                substitutionContext, callResultsCacheFactory, callResultResolverFactory);

            var specimen = new object();
            var specimenContext = Substitute.For<ISpecimenContext>();
            var callRouter = new CallRouterStub();
            substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            var expectedResultResolver = Substitute.For<ICallResultResolver>();
            callResultResolverFactory.Create(specimenContext).Returns(expectedResultResolver);

            // Act
            sut.Execute(specimen, specimenContext);

            // Assert
            var handler =
                (AutoFixtureValuesHandler) callRouter.RegisteredFactory.Invoke(Substitute.For<ISubstituteState>());
            Assert.Equal(expectedResultResolver, handler.ResultResolver);
        }


        [Fact]
        public void ShouldSilentlySkipNotASubstituteSpecimen()
        {
            // Arrange
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext);

            var specimen = new object();
            var context = Substitute.For<ISpecimenContext>();

            substitutionContext.When(x => x.GetCallRouterFor(specimen)).Throw<NotASubstituteException>();

            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(specimen, context)));
        }

        [Fact]
        public void ResultCacheAndCallResultResolverShouldBeSameForEachHandler()
        {
            // Arrange
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext);

            var specimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            var substituteState = Substitute.For<ISubstituteState>();

            var callRouter = new CallRouterStub();
            substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            sut.Execute(specimen, context);

            // Act
            var instance1 = callRouter.RegisteredFactory.Invoke(substituteState);
            var instance2 = callRouter.RegisteredFactory.Invoke(substituteState);

            // Assert
            var handler1 = (AutoFixtureValuesHandler) instance1;
            var handler2 = (AutoFixtureValuesHandler) instance2;

            Assert.NotSame(handler1, handler2);
            Assert.Same(handler1.ResultResolver, handler2.ResultResolver);
            Assert.Same(handler1.ResultCache, handler2.ResultCache);
        }

        [Fact]
        public void ShouldPassCorrectSpecimenContextToCallResultResolver()
        {
            // Arrange
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext);

            var specimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            var substituteState = Substitute.For<ISubstituteState>();

            var callRouter = new CallRouterStub();
            substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            sut.Execute(specimen, context);

            // Act
            var handler = (AutoFixtureValuesHandler) callRouter.RegisteredFactory.Invoke(substituteState);

            // Assert
            Assert.Equal(context, ((CallResultResolver) handler.ResultResolver).SpecimenContext);
        }

        [Fact]
        public void ValidCallSpecificationFactoryIsPassedToHandler()
        {
            // Arrange
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext);

            var specimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            var substituteState = Substitute.For<ISubstituteState>();

            var callRouter = new CallRouterStub();
            substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            var callSpecFactory = Substitute.For<ICallSpecificationFactory>();
            substituteState.CallSpecificationFactory.Returns(callSpecFactory);

            sut.Execute(specimen, context);

            // Act
            var handler = (AutoFixtureValuesHandler) callRouter.RegisteredFactory.Invoke(substituteState);

            // Assert
            Assert.Equal(callSpecFactory, handler.CallSpecificationFactory);
        }


        /// <summary>
        /// It's required to create custom mock, because NSubstitute cannot create fully functional
        /// mock for the ICallRouter interface - it plays a special role inside NSubstitute.
        /// </summary>
        private class CallRouterStub : ICallRouter
        {
            public CallHandlerFactory RegisteredFactory { get; private set; }

            public bool IsLastCallInfoPresent() => throw new NotImplementedException();

            public ConfiguredCall LastCallShouldReturn(IReturn returnValue, MatchArgs matchArgs) =>
                throw new NotImplementedException();

            public object Route(ICall call) => throw new NotImplementedException();

            public IEnumerable<ICall> ReceivedCalls() => throw new NotImplementedException();

            public void SetRoute(Func<ISubstituteState, IRoute> getRoute) => throw new NotImplementedException();

            public void SetReturnForType(Type type, IReturn returnValue) => throw new NotImplementedException();

            public void RegisterCustomCallHandlerFactory(CallHandlerFactory factory)
            {
                this.RegisteredFactory = factory;
            }

            public void Clear(ClearOptions clear) => throw new NotImplementedException();
        }
    }
}