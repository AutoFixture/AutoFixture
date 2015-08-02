using System.Collections.Generic;
using System.Linq;
using Moq;
using Ploeh.TestTypeFoundation;
using Xunit;
using System;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class FixtureIntegrationWithAutoMoqCustomizationTest
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
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateMockOfAction()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Exercise system
            var actual = fixture.Create<Mock<Action<string>>>();
            // Verify outcome
            Assert.NotNull(actual);
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateUsableMockOfFunc()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expected = fixture.Create<Version>();
            var mockOfFunc = fixture.Create<Mock<Func<int, Version>>>();
            mockOfFunc.Setup(f => f(42)).Returns(expected);

            // Exercise system
            var actual = mockOfFunc.Object(42);
            
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void FixtureCanFreezeUsableMockOfFunc()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expected = fixture.Create<Uri>();
            var mockOfFunc = fixture.Freeze<Mock<Func<Guid, decimal, Uri>>>();
            mockOfFunc
                .Setup(f => f(It.IsAny<Guid>(), 1337m))
                .Returns(expected);

            // Exercise system
            var actual = mockOfFunc.Object(Guid.NewGuid(), 1337m);

            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateUsableMockOfCustomDelegate()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expected = fixture.Create<string>();
            var mockOfDelegate = fixture.Create<Mock<DBaz>>();
            mockOfDelegate.Setup(f => f(13, 37)).Returns(expected);

            // Exercise system
            var actual = mockOfDelegate.Object(13, 37);

            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        public interface IFoo
        {
            IBar Bar { get; set; }
        }

        public interface IBar
        {
        }

        public delegate string DBaz(short s, byte b);
    }
}
