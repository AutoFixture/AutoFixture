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
            var result = fixture.CreateAnonymous<IInterface>();
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
            var result = fixture.CreateAnonymous<AbstractType>();
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
            var result = fixture.CreateAnonymous<AbstractTypeWithNonDefaultConstructor<int>>();
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
            var result = fixture.CreateAnonymous<Mock<AbstractType>>();
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
            var result = fixture.CreateAnonymous<IInterface>();
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
            var result = fixture.CreateAnonymous<IList<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void TestMethod1()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var instance1 = fixture.CreateAnonymous<Mock<IFoo>>();
            Assert.NotNull(instance1.Object.Bar);//Passed => OK
            var instance2 = fixture.Build<Mock<IFoo>>().OmitAutoProperties().CreateAnonymous();
            Assert.Null(instance2.Object.Bar);//Passed => OK
            var instance3 = fixture.Build<Mock<IFoo>>().OmitAutoProperties().CreateAnonymous();
            Assert.Null(instance3.Object.Bar);//Passed => OK
        }

        [Fact]
        public void Test2()
        {
            var instance2 = new Mock<IFoo>();
            Assert.Null(instance2.Object.Bar);//Passed => OK
            var instance3 = new Mock<IFoo>();
            Assert.Null(instance3.Object.Bar);//Not Failed => ???
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
