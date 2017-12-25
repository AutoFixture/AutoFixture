using System.Linq;
using System.Reflection;
using AutoFixture.AutoNSubstitute.CustomCallHandler;
using AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using AutoFixture.Kernel;
using NSubstitute;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public class CallResultResolverTest
    {
        [Fact]
        public void ShouldUseTypeRequestToResolveReturnValue()
        {
            // Arrange
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            var returnValue = "returnResult";
            specimenContext.Resolve(typeof(string)).Returns(returnValue);

            // Act
            var callResult = sut.ResolveResult(call);

            // Assert
            Assert.True(callResult.ReturnValue.HasValue());
            Assert.Equal(returnValue, callResult.ReturnValue.ValueOrDefault());
        }

        [Fact]
        public void ShouldUsePropertyInfoRequestToResolvePropertyValue()
        {
            // Arrange
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithProperty>();
            var call = CallHelper.CreatePropertyGetterCallMock(() => target.Property);

            var propertyInfo = typeof(IInterfaceWithProperty).GetProperty(nameof(IInterfaceWithProperty.Property));

            var retValue = "returnValue";
            specimenContext.Resolve(propertyInfo).Returns(retValue);

            // Act
            var result = sut.ResolveResult(call);

            // Assert
            Assert.True(result.ReturnValue.HasValue());
            Assert.Equal(retValue, result.ReturnValue.ValueOrDefault());
        }

        [Fact]
        public void ShouldResolveOutParameterValue()
        {
            // Arrange
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithParameterAndOutVoidMethod>();
            int _;
            var call = CallHelper.CreateCallMock(() => target.Method(null, out _));

            specimenContext.Resolve(typeof(int)).Returns(42);

            // Act
            var callResult = sut.ResolveResult(call);

            // Assert
            Assert.Single(callResult.ArgumentValues);
            Assert.Equal(1, callResult.ArgumentValues.First().Index);
            Assert.Equal(42, callResult.ArgumentValues.First().Value);
        }

        [Fact]
        public void ShouldResolveRefParameterValue()
        {
            // Arrange
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithRefIntMethod>();
            int passedValue = 1;
            var call = CallHelper.CreateCallMock(() => target.Method(ref passedValue));

            specimenContext.Resolve(typeof(int)).Returns(42);

            // Act
            var callResult = sut.ResolveResult(call);

            // Assert
            Assert.Single(callResult.ArgumentValues);
            Assert.Equal(0, callResult.ArgumentValues.First().Index);
            Assert.Equal(42, callResult.ArgumentValues.First().Value);
        }


        [Fact]
        public void ShouldNotReturnValueIfOmitted()
        {
            // Arrange
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithParameterlessMethod>();
            var call = CallHelper.CreateCallMock(() => target.Method());

            specimenContext.Resolve(typeof(string)).Returns(new OmitSpecimen());

            // Act
            var result = sut.ResolveResult(call);

            // Assert
            Assert.False(result.ReturnValue.HasValue());
        }

        [Fact]
        public void ShouldNotResolveArgumentValueIfOmitted()
        {
            // Arrange
            var specimenContext = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolver(specimenContext);

            var target = Substitute.For<IInterfaceWithRefIntMethod>();

            int _ = 0;
            var call = CallHelper.CreateCallMock(() => target.Method(ref _));

            specimenContext.Resolve(typeof(int)).Returns(new OmitSpecimen());

            // Act
            var result = sut.ResolveResult(call);

            // Assert
            Assert.True(result.ReturnValue.HasValue());
            Assert.Empty(result.ArgumentValues);
        }
    }
}