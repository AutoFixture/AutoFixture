using NSubstitute;
using Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class CallResultResolverTest
    {
        private readonly Fixture _fixture;
        private readonly ISpecimenContext _specimenContext;
        private readonly CallResultResolver _sut;

        public CallResultResolverTest()
        {
            _fixture = new Fixture();
            _specimenContext = Substitute.For<ISpecimenContext>();
            _sut = new CallResultResolver(_specimenContext);
        }

        [Fact]
        public void ReturnValueIsResolvedViaTypeRequest()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var returnValue = _fixture.Create("result");
            _specimenContext.Resolve(typeof(string)).Returns(returnValue);

            //act
            var callResult = _sut.ResolveResult(call);

            //assert
            Assert.True(callResult.ReturnValue.HasValue());
            Assert.Equal(returnValue, callResult.ReturnValue.ValueOrDefault());
        }

        [Fact]
        public void PropertyInfoRequestGeneredForProperties()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithProperty>();
            var call = CallHelper.CreatePropertyCallMock(() => target.Property);
            var propertyInfo = typeof(IInterfaceWithProperty).GetProperty(nameof(IInterfaceWithProperty.Property));

            var retValue = _fixture.Create<string>();
            _specimenContext.Resolve(propertyInfo).Returns(retValue);

            //act
            var result = _sut.ResolveResult(call);

            //assert
            Assert.True(result.ReturnValue.HasValue());
            Assert.Equal(retValue, result.ReturnValue.ValueOrDefault());
        }

        [Fact]
        public void NonReferenceArgumentsAreSkipped()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterAndOutVoidMethod>();
            int _;
            var call = CallHelper.CreateCallMock(() => target.Method(null,out _));

            //act
            var callResult = _sut.ResolveResult(call);

            //assert
            Assert.Equal(1, callResult.ArgumentValues.Length);
            Assert.Equal(1, callResult.ArgumentValues[0].Item1);
        }

        [Fact]
        public void DoesntReturnResultIfOmitted()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            _specimenContext.Resolve(typeof(string)).Returns(new OmitSpecimen());

            //act
            var result = _sut.ResolveResult(call);

            //assert
            Assert.False(result.ReturnValue.HasValue());
        }

        [Fact]
        public void ArgumentsAreNotReturnedIfOmitted()
        {
            //arrange
            var target = Substitute.For<IInterfaceWithRefIntMethod>();

            int _ = 0;
            var call = CallHelper.CreateCallMock(() => target.Method(ref _));

            _specimenContext.Resolve(typeof(int)).Returns(new OmitSpecimen());

            //act
            var result = _sut.ResolveResult(call);

            //assert
            Assert.True(result.ReturnValue.HasValue());
            Assert.Empty(result.ArgumentValues);
        }
    }
}