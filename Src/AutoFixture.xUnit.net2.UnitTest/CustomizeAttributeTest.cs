using System;
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
        public void SutImplementsIParameterCustomizationSource()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingCustomizeAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<IParameterCustomizationSource>(sut);
            // Teardown
        }
    }
}
