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
            // Fixture setup
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var callResultsCacheFactory = Substitute.For<ICallResultCacheFactory>();
            var callResultResolverFactory = Substitute.For<ICallResultResolverFactory>();

            // Excercise system
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext, callResultsCacheFactory,
                callResultResolverFactory);

            // Verify outcome
            Assert.Equal(substitutionContext, sut.SubstitutionContext);
            Assert.Equal(callResultsCacheFactory, sut.CallResultCacheFactory);
            Assert.Equal(callResultResolverFactory, sut.CallResultResolverFactory);

            // Teardown
        }

        [Fact]
        public void ShouldUseConstructorDefaultsOfCorrectType()
        {
            // Fixture setup
            var substitutionContext = Substitute.For<ISubstitutionContext>();

            // Excercise system
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext);

            // Verify outcome
            Assert.IsType<CallResultCacheFactory>(sut.CallResultCacheFactory);
            Assert.IsType<CallResultResolverFactory>(sut.CallResultResolverFactory);

            // Teardown
        }

        [Fact]
        public void ShouldUseValueFromCallResultsCacheFactory()
        {
            // Fixture setup
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

            // Excercise system
            sut.Execute(specimen, specimenContext);

            // Verify outcome
            var handler =
                (AutoFixtureValuesHandler) callRouter.RegisteredFactory.Invoke(Substitute.For<ISubstituteState>());
            Assert.Equal(expectedResultsCache, handler.ResultCache);

            // Teardown
        }

        [Fact]
        public void ShouldUseValueFromCallResultResolverFactory()
        {
            // Fixture setup
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

            // Excercise system
            sut.Execute(specimen, specimenContext);

            // Verify outcome
            var handler =
                (AutoFixtureValuesHandler) callRouter.RegisteredFactory.Invoke(Substitute.For<ISubstituteState>());
            Assert.Equal(expectedResultResolver, handler.ResultResolver);

            // Teardown
        }


        [Fact]
        public void ShouldSilentlySkipNotASubstituteSpecimen()
        {
            // Fixture setup
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext);

            var specimen = new object();
            var context = Substitute.For<ISpecimenContext>();

            substitutionContext.When(x => x.GetCallRouterFor(specimen)).Throw<NotASubstituteException>();

            // Exercise system & Verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(specimen, context)));

            // Teardown
        }

        [Fact]
        public void ResultCacheAndCallResultResolverShouldBeSameForEachHandler()
        {
            // Fixture setup
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext);

            var specimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            var substituteState = Substitute.For<ISubstituteState>();

            var callRouter = new CallRouterStub();
            substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            sut.Execute(specimen, context);

            // Exercise system
            var instance1 = callRouter.RegisteredFactory.Invoke(substituteState);
            var instance2 = callRouter.RegisteredFactory.Invoke(substituteState);

            // Verify outcome
            var handler1 = (AutoFixtureValuesHandler) instance1;
            var handler2 = (AutoFixtureValuesHandler) instance2;

            Assert.NotSame(handler1, handler2);
            Assert.Same(handler1.ResultResolver, handler2.ResultResolver);
            Assert.Same(handler1.ResultCache, handler2.ResultCache);

            // Teardown
        }

        [Fact]
        public void ShouldPassCorrectSpecimenContextToCallResultResolver()
        {
            // Fixture setup
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext);

            var specimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            var substituteState = Substitute.For<ISubstituteState>();

            var callRouter = new CallRouterStub();
            substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            sut.Execute(specimen, context);

            // Exercise system
            var handler = (AutoFixtureValuesHandler) callRouter.RegisteredFactory.Invoke(substituteState);

            // Verify outcome
            Assert.Equal(context, ((CallResultResolver) handler.ResultResolver).SpecimenContext);

            // Teardown
        }

        [Fact]
        public void ValidCallSpecificationFactoryIsPassedToHandler()
        {
            // Fixture setup
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

            // Exercise system
            var handler = (AutoFixtureValuesHandler) callRouter.RegisteredFactory.Invoke(substituteState);

            // Verify outcome
            Assert.Equal(callSpecFactory, handler.CallSpecificationFactory);

            // Teardown
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