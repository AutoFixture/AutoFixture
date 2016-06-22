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
            var result = fixture.Create<IInterface>();
            // Verify outcome
            Assert.IsAssignableFrom<IInterface>(result);
            // Teardown
        }

        [Fact]
        public void FixtureAutoFakesAbstractType()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.Create<AbstractType>();
            // Verify outcome
            Assert.IsAssignableFrom<AbstractType>(result);
            // Teardown
        }

        [Fact]
        public void FixtureCanPassValuesToAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
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
            var result = fixture.Create<Fake<AbstractType>>();
            // Verify outcome
            Assert.IsAssignableFrom<Fake<AbstractType>>(result);
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
            var result = fixture.Create<IInterface>();
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
            var result = fixture.Create<IList<ConcreteType>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.Create<AbstractGenericType<object>>();
            // Verify outcome
            Assert.IsAssignableFrom<AbstractGenericType<object>>(result);
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithConstructorWithMultipleParameters()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>();
            // Verify outcome
            Assert.IsAssignableFrom<AbstractTypeWithConstructorWithMultipleParameters<int, int>>(result);
        }

        [Fact]
        public void FixtureCanCreateAnonymousGuid()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Exercise system
            var result = fixture.Create<Guid>();
            // Verify outcome
            Assert.NotEqual(Guid.Empty, result);
            // Teardown
        }
    }
}