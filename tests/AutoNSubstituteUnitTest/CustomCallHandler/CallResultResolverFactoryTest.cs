using AutoFixture.AutoNSubstitute.CustomCallHandler;
using AutoFixture.Kernel;
using NSubstitute;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public class CallResultResolverFactoryTest
    {
        [Fact]
        public void CreateReturnsResultOfCorrectType()
        {
            // Arrange
            var context = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolverFactory();

            // Act
            var result = sut.Create(context);

            // Assert
            Assert.IsType<CallResultResolver>(result);
        }

        [Fact]
        public void CreateShouldPassValueToConstructor()
        {
            // Arrange
            var context = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolverFactory();

            // Act
            var result = sut.Create(context);

            // Assert
            var resolver = Assert.IsType<CallResultResolver>(result);
            Assert.Same(context, resolver.SpecimenContext);
        }
    }
}