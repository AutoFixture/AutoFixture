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
        public void FixtureAutoFakesInterface()
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
        public void FixtureAutoFakesAbstractType()
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
        public void FixtureAutoFakesAbstractTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractTypeWithNonDefaultConstructor<int>>();
            // Verify outcome
            Assert.NotEqual(0, result.Property);
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateFake()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<Fake<AbstractType>>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void FixtureCanFreezeFake()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            var dummy = new object();
            var fake = fixture.Freeze<Fake<IInterface>>();
            fake.CallsTo(x => x.MakeIt(dummy))
                .Returns(null);
            // Exercise system
            var result = fixture.CreateAnonymous<IInterface>();
            result.MakeIt(dummy);
            // Verify outcome
            A.CallTo(() => result.MakeIt(dummy)).MustHaveHappened();
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