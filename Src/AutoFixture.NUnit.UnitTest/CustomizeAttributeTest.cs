using System;
using Xunit;

namespace Ploeh.AutoFixture.NUnit.UnitTest
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
    }
}
