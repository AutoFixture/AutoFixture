using System;
using System.Collections.Generic;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Exceptions;
using NSubstitute.Routing;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class NSubstituteRegisterCallHandlerCommandTest
    {
        private readonly IFixture _fixture;
        private readonly ISubstitutionContext _substitutionContext;
        private readonly NSubstituteRegisterCallHandlerCommand _sut;

        public NSubstituteRegisterCallHandlerCommandTest()
        {
            _fixture = new Fixture();
            _substitutionContext = Substitute.For<ISubstitutionContext>();
            _sut = new NSubstituteRegisterCallHandlerCommand(_substitutionContext);
        }

        [Fact]
        public void NonSubstitutionAreSilentlySkipped()
        {
            //arrange
            var specimen = _fixture.Create<object>();
            var context = Substitute.For<ISpecimenContext>();

            _substitutionContext.When(x => x.GetCallRouterFor(specimen)).Throw<NotASubstituteException>();

            //act & assert
            Assert.DoesNotThrow(() => _sut.Execute(specimen, context));
        }

        [Fact]
        public void ResultCacheAndCallResultResolverAreSameForAllCallHandlers()
        {
            //arrange
            var specimen = _fixture.Create<object>();
            var context = Substitute.For<ISpecimenContext>();
            var substituteState = Substitute.For<ISubstituteState>();

            var callRouter = new CallRouterMock();
            _substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            _sut.Execute(specimen, context);

            //act
            var instance1 = callRouter.RegisteredFactory.Invoke(substituteState);
            var instance2 = callRouter.RegisteredFactory.Invoke(substituteState);

            //assert
            var handler1 = (AutoFixtureValuesHandler) instance1;
            var handler2 = (AutoFixtureValuesHandler) instance2;

            Assert.Same(handler1.ResultResolver, handler2.ResultResolver);
            Assert.Same(handler1.ResultsCache, handler2.ResultsCache);
        }

        [Fact]
        public void ValidSpecimenContextPassedToCallResultResolver()
        {
            //arrange
            var specimen = _fixture.Create<object>();
            var context = Substitute.For<ISpecimenContext>();
            var substituteState = Substitute.For<ISubstituteState>();

            var callRouter = new CallRouterMock();
            _substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            _sut.Execute(specimen, context);

            //act
            var handler = (AutoFixtureValuesHandler) callRouter.RegisteredFactory.Invoke(substituteState);

            //assert
            Assert.Equal(context, ((CallResultResolver) handler.ResultResolver).SpecimenContext);
        }


        [Fact]
        public void ValidCallSpecificationFactoryIsPassedToHandler()
        {
            //arrange
            var specimen = _fixture.Create<object>();
            var context = Substitute.For<ISpecimenContext>();
            var substituteState = Substitute.For<ISubstituteState>();

            var callRouter = new CallRouterMock();
            _substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            var callSpecFactory = Substitute.For<ICallSpecificationFactory>();
            substituteState.CallSpecificationFactory.Returns(callSpecFactory);

            _sut.Execute(specimen, context);

            //act
            var handler = (AutoFixtureValuesHandler) callRouter.RegisteredFactory.Invoke(substituteState);

            //assert
            Assert.Equal(callSpecFactory, handler.CallSpecificationFactory);
        }

        /// <summary>
        /// It's required to create custom mock, because NSubstitute cannot create fully functional
        /// mock for the ICallRouter interface - it plays a special role inside NSubstitute.
        /// </summary>
        private class CallRouterMock : ICallRouter
        {
            public CallHandlerFactory RegisteredFactory { get; private set; }

            public bool IsLastCallInfoPresent()
            {
                throw new NotImplementedException();
            }

            public ConfiguredCall LastCallShouldReturn(IReturn returnValue, MatchArgs matchArgs)
            {
                throw new NotImplementedException();
            }

            public object Route(ICall call)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<ICall> ReceivedCalls()
            {
                throw new NotImplementedException();
            }

            public void SetRoute(Func<ISubstituteState, IRoute> getRoute)
            {
                throw new NotImplementedException();
            }

            public void SetReturnForType(Type type, IReturn returnValue)
            {
                throw new NotImplementedException();
            }

            public void RegisterCustomCallHandlerFactory(CallHandlerFactory factory)
            {
                RegisteredFactory = factory;
            }

            public void Clear(ClearOptions clear)
            {
                throw new NotImplementedException();
            }
        }
    }
}