using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.Xunit2.UnitTest
{
    public class CustomizeAttributeTest
    {
        [Fact]
        public void TestableSutIsSut()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingCustomizeAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingCustomizeAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<Attribute>(sut);
            // Teardown
        }

        [Fact]
        public void SutImplementsIParameterCustomizationProviderToBeRecognizedByTestDataProvider()
        {
            // Fixture setup
            // Exercise system 
            // Verify outcome
            Assert.True(typeof(IParameterCustomizationProvider).IsAssignableFrom(typeof(CustomizeAttribute)));
            // Teardown
        }
    }
}
