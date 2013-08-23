using System;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.org;

namespace Ploe.AutoFixture.NUnit.org.UnitTest
{
    [TestFixture]
    public class CustomizeAttributeTest
    {
        [Test]
        public void TestableSutIsSut()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingCustomizeAttribute();
            // Verify outcome
            Assert.IsInstanceOf<CustomizeAttribute>(sut);
            // Teardown
        }

        [Test]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingCustomizeAttribute();
            // Verify outcome
            Assert.IsInstanceOf<Attribute>(sut);
            // Teardown
        }
    }
}
