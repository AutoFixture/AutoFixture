using System;
using System.Reflection;
using Ploeh.TestTypeFoundation;
using Xunit;
using FakeItEasy;

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
    }
}
