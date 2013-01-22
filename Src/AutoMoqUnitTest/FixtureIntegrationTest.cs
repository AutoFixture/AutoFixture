using System.Collections.Generic;
using System.Linq;
using Moq;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class FixtureIntegrationTest
    {
        [Fact]
        public void FixtureAutoMocksInterface()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Exercise system
            var result = fixture.Create<IInterface>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksAbstractType()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Exercise system
            var result = fixture.Create<AbstractType>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksAbstractTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Exercise system
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
            // Verify outcome
            Assert.NotEqual(0, result.Property);
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateMock()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Exercise system
            var result = fixture.Create<Mock<AbstractType>>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void FixtureCanFreezeMock()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var expected = new object();

            fixture.Freeze<Mock<IInterface>>()
                .Setup(a => a.MakeIt(It.IsAny<object>()))
                .Returns(expected);
            // Exercise system
            var result = fixture.Create<IInterface>();
            // Verify outcome
            var dummy = new object();
            Assert.Equal(expected, result.MakeIt(dummy));
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Exercise system
            var result = fixture.Create<IList<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        public interface IFoo
        {
            IBar Bar { get; set; }
        }

        public interface IBar
        {
        }
    }
}
