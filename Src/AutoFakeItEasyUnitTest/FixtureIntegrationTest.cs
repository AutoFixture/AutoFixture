using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FixtureIntegrationTest
    {
        [Fact]
        public void FixtureAutoMocksInterface()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
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
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void FixtureCanFreezeMock()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            var dummy = new object();
            var expected = new object();
            A.CallTo(() => fixture.Freeze<Fake<IInterface>>().FakedObject.MakeIt(dummy))
                .Returns(expected);
            // Exercise system
            var result = fixture.CreateAnonymous<IInterface>();
            // Verify outcome
            Assert.Equal(expected, result.MakeIt(dummy));
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<IList<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractGenericType<object>>();
            // Verify outcome
            Assert.NotNull(result);
        }

        [Fact]
        public void FixtureCanCreateAnonymousGuid()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<Guid>();
            // Verify outcome
            Assert.NotEqual(Guid.Empty, result);
            // Teardown
        }
    }
}