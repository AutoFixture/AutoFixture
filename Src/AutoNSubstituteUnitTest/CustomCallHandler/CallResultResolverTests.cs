using System.Linq;
using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture.AutoNSubstitute.CustomCallHandler;
using Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public class CallResultResolverTest
    {
        [Fact]
        public void ShouldUseTypeRequestToResolveReturnValue()
        {
            // Fixture setup
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var returnValue = "returnResult";
            specimenContext.Resolve(typeof(string)).Returns(returnValue);

            // Exercise system
            var callResult = sut.ResolveResult(call);

            // Verify outcome
            Assert.True(callResult.ReturnValue.HasValue());
            Assert.Equal(returnValue, callResult.ReturnValue.ValueOrDefault());

            // Teardown
        }

        [Fact]
        public void ShouldUsePropertyInfoRequestToResolvePropertyValue()
        {
            // Fixture setup
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithProperty>();
            var call = CallHelper.CreatePropertyGetterCallMock(() => target.Property);

            var propertyInfo = typeof(IInterfaceWithProperty).GetProperty(nameof(IInterfaceWithProperty.Property));

            var retValue = "returnValue";
            specimenContext.Resolve(propertyInfo).Returns(retValue);

            // Exercise system
            var result = sut.ResolveResult(call);

            // Verify outcome
            Assert.True(result.ReturnValue.HasValue());
            Assert.Equal(retValue, result.ReturnValue.ValueOrDefault());

            // Teardown
        }

        [Fact]
        public void ShouldResolveOutParameterValue()
        {
            // Fixture setup
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithParameterAndOutVoidMethod>();
            int _;
            var call = CallHelper.CreateCallMock(() => target.Method(null, out _));

            specimenContext.Resolve(typeof(int)).Returns(42);

            // Exercise system
            var callResult = sut.ResolveResult(call);

            // Verify outcome
            Assert.Equal(1, callResult.ArgumentValues.Count());
            Assert.Equal(1, callResult.ArgumentValues.First().Index);
            Assert.Equal(42, callResult.ArgumentValues.First().Value);

            // Teardown
        }

        [Fact]
        public void ShouldResolveRefParameterValue()
        {
            // Fixture setup
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithRefIntMethod>();
            int passedValue = 1;
            var call = CallHelper.CreateCallMock(() => target.Method(ref passedValue));

            specimenContext.Resolve(typeof(int)).Returns(42);

            // Exercise system
            var callResult = sut.ResolveResult(call);

            // Verify outcome
            Assert.Equal(1, callResult.ArgumentValues.Count());
            Assert.Equal(0, callResult.ArgumentValues.First().Index);
            Assert.Equal(42, callResult.ArgumentValues.First().Value);

            // Teardown
        }


        [Fact]
        public void ShouldNotReturnValueIfOmitted()
        {
            // Fixture setup
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            specimenContext.Resolve(typeof(string)).Returns(new OmitSpecimen());

            // Exercise system
            var result = sut.ResolveResult(call);

            // Verify outcome
            Assert.False(result.ReturnValue.HasValue());

            // Teardown
        }

        [Fact]
        public void ShouldNotResolveArgumentValueIfOmitted()
        {
            // Fixture setup
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithRefIntMethod>();

            int _ = 0;
            var call = CallHelper.CreateCallMock(() => target.Method(ref _));

            specimenContext.Resolve(typeof(int)).Returns(new OmitSpecimen());

            // Exercise system
            var result = sut.ResolveResult(call);

            // Verify outcome
            Assert.True(result.ReturnValue.HasValue());
            Assert.Empty(result.ArgumentValues);

            // Teardown
        }
    }
}