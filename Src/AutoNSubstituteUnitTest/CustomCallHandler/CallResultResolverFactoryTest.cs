using NSubstitute;
using Ploeh.AutoFixture.AutoNSubstitute.CustomCallHandler;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public class CallResultResolverFactoryTest
    {
        [Fact]
        public void CreateReturnsResultOfCorrectType()
        {
            // Fixture setup
            var context = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolverFactory();

            // Exercise system
            var result = sut.Create(context);

            // Verify outcome
            Assert.IsType<CallResultResolver>(result);
            // Teardown
        }

        [Fact]
        public void CreateShouldPassValueToConstructor()
        {
            // Fixture setup
            var context = Substitute.For<ISpecimenContext>();
            var sut = new CallResultResolverFactory();

            // Exercise system
            var result = sut.Create(context);

            // Verify outcome
            var resolver = Assert.IsType<CallResultResolver>(result);
            Assert.Same(context, resolver.SpecimenContext);
            // Teardown
        }
    }
}