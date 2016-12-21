using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class AutoFixtureValuesHandlerTest
    {
        private readonly Fixture _fixture;
        private readonly ISpecimenContext _specimenContext;
        private readonly IResultsCache _resultsCache;
        private readonly ICallSpecificationFactory _callSpecificationFactory;
        private readonly AutoFixtureValuesHandler _sut;

        public AutoFixtureValuesHandlerTest()
        {
            _fixture = new Fixture();
            _specimenContext = Substitute.For<ISpecimenContext>();
            _resultsCache = Substitute.For<IResultsCache>();
            _callSpecificationFactory = Substitute.For<ICallSpecificationFactory>();

            _sut = new AutoFixtureValuesHandler(_specimenContext, _resultsCache, _callSpecificationFactory);
        }

        private static ICall CreateCall(Expression<Action> method, object[] originalArgs = null)
        {
            var methodExpression = (MethodCallExpression) method.Body;
            var args = new List<object>();
            foreach (var argExpression in methodExpression.Arguments)
            {
                var value = Expression.Lambda(argExpression).Compile().DynamicInvoke();
                args.Add(value);
            }

            var argsArray = args.ToArray();
            var methodInfo = methodExpression.Method;

            var call = Substitute.For<ICall>();
            call.GetMethodInfo().Returns(methodInfo);
            call.GetReturnType().Returns(methodInfo.ReturnType);
            call.GetArguments().Returns(argsArray);
            call.GetOriginalArguments().Returns(originalArgs ?? argsArray);

            return call;
        }

        private static ICall CreatePropertyCall<T>(Expression<Func<T>> getProp)
        {
            var propertyInfo = (PropertyInfo) ((MemberExpression) getProp.Body).Member;

            var call = Substitute.For<ICall>();
            call.GetMethodInfo().Returns(propertyInfo.GetMethod);
            call.GetReturnType().Returns(propertyInfo.PropertyType);
            call.GetArguments().Returns(new object[0]);
            call.GetOriginalArguments().Returns(new object[0]);

            return call;
        }

        [Fact]
        public void ReturnValueIsResolvedViaTypeRequest()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CreateCall(() => target.Method());

            var returnValue = _fixture.Create("result");
            _specimenContext.Resolve(typeof(string)).Returns(returnValue);

            //act
            var callResult = _sut.Handle(call);

            //assert
            Assert.True(callResult.HasReturnValue);
            Assert.Equal(returnValue, callResult.ReturnValue);
        }

        [Fact]
        public void ResultValueIsCachedWithCorrectSpecs()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CreateCall(() => target.Method());

            var callSpec = Substitute.For<ICallSpecification>();
            _callSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall).Returns(callSpec);

            //act
            _sut.Handle(call);

            //assert
            _resultsCache.Received().AddResult(callSpec, Arg.Any<CachedCallResult>());
        }

        [Fact]
        public void ResolvedValueIsCached()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CreateCall(() => target.Method());

            var returnValue = _fixture.Create("result");
            _specimenContext.Resolve(typeof(string)).Returns(returnValue);

            //act
            _sut.Handle(call);

            //assert
            _resultsCache.Received()
                .AddResult(
                    Arg.Any<ICallSpecification>(),
                    Arg.Is<CachedCallResult>(x => x.ReturnValue == (object) returnValue));
        }

        [Fact]
        public void ReturnsValueFromCacheIfPresent()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CreateCall(() => target.Method());

            var callSpec = Substitute.For<ICallSpecification>();
            _callSpecificationFactory.CreateFrom(call, MatchArgs.AsSpecifiedInCall).Returns(callSpec);

            var cachedResult = _fixture.Create("stringRetValue");

            CachedCallResult _;
            _resultsCache.TryGetResult(call, out _)
                .Returns(c =>
                {
                    c[1] = new CachedCallResult(cachedResult, new Tuple<int, object>[0]);
                    return true;
                });

            //act
            var actualResult = _sut.Handle(call);

            //assert
            Assert.True(actualResult.HasReturnValue);
            Assert.Equal(cachedResult, actualResult.ReturnValue);
        }

        [Fact]
        public void RefArgumentIsSet()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithRefIntMethod>();
            int _ = 0;
            var call = CreateCall(() => target.Method(ref _));
            var callArgs = call.GetArguments();

            var intValue = _fixture.Create<int>();
            _specimenContext.Resolve(typeof(int)).Returns(intValue);

            //act
            _sut.Handle(call);

            //assert
            Assert.Equal(intValue, callArgs[0]);
        }

        [Fact]
        public void OutArgumentIsSet()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithOutVoidMethod>();
            int _;
            var call = CreateCall(() => target.Method(out _));
            var callArgs = call.GetArguments();

            var intValue = _fixture.Create<int>();
            _specimenContext.Resolve(typeof(int)).Returns(intValue);

            //act
            _sut.Handle(call);

            //assert
            Assert.Equal(intValue, callArgs[0]);
        }

        [Fact]
        public void ModifiedRefArgumentIsNotUpdated()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithRefVoidMethod>();

            int origValue = 0;
            var call = CreateCall(() => target.Method(ref origValue), new object[] {100});
            var callArgs = call.GetArguments();

            var intValue = _fixture.Create<int>();
            _specimenContext.Resolve(typeof(int)).Returns(intValue);

            //act
            _sut.Handle(call);

            //assert
            Assert.Equal(origValue, callArgs[0]);
        }

        [Fact]
        public void PropertyInfoRequestGeneredForProperties()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithProperty>();
            var call = CreatePropertyCall(() => target.Property);
            var propertyInfo = typeof(IInterfaceWithProperty).GetProperty(nameof(IInterfaceWithProperty.Property));

            var retValue = _fixture.Create<string>();
            _specimenContext.Resolve(propertyInfo).Returns(retValue);

            //act
            var result = _sut.Handle(call);

            //assert
            Assert.True(result.HasReturnValue);
            Assert.Equal(retValue, result.ReturnValue);
        }

        [Fact]
        public void DoesntReturnResultIfOmitted()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CreateCall(() => target.Method());

            _specimenContext.Resolve(typeof(string)).Returns(new OmitSpecimen());

            //act
            var result = _sut.Handle(call);

            //assert
            Assert.False(result.HasReturnValue);
        }

        [Fact]
        public void RefArgumentsAreNotSetIfOmitted()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithRefIntMethod>();
            int origValue = _fixture.Create<int>();
            var call = CreateCall(() => target.Method(ref origValue));
            var callArgs = call.GetArguments();

            _specimenContext.Resolve(typeof(int)).Returns(new OmitSpecimen());

            //act
            var result = _sut.Handle(call);

            //assert
            Assert.True(result.HasReturnValue);
            Assert.Equal(origValue, callArgs[0]);
        }

        [Fact]
        public void PropertyIsNotSetIfOmitted()
        {
            var target = Substitute.For<IInterfaceWithProperty>();
            var call = CreatePropertyCall(() => target.Property);

            _specimenContext.Resolve(Arg.Any<object>()).Returns(new OmitSpecimen());

            //act
            var result = _sut.Handle(call);

            //assert
            Assert.False(result.HasReturnValue);
        }
    }
}